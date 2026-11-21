import hashlib
import json
import re
import sys
import unicodedata
import xml.etree.ElementTree as ET
import zipfile
from pathlib import Path

import pymupdf

ROOT = Path(__file__).resolve().parent
SOURCES = ROOT / "sources"
OUTPUT = ROOT / "knowledge-base.json"

MAX_CHUNK = 1500
MIN_CHUNK = 100

ACT_ARTICLES = ["1", "2", "3", "35", "36", "37", "38", "71", "81", "82", "83", "84", "89", "108"]

W = "{http://schemas.openxmlformats.org/wordprocessingml/2006/main}"
MC_FALLBACK = "{http://schemas.openxmlformats.org/markup-compatibility/2006}Fallback"
X = "{http://schemas.openxmlformats.org/spreadsheetml/2006/main}"

LOWER_START = re.compile(r"^[a-ząćęłńóśźż]")

CHARACTER_FIXES = {0xA0: " ", 0xF0B7: "•", 0xAD: None, 0xFEFF: None}


def slug(text):
    text = unicodedata.normalize("NFKD", text.replace("ł", "l").replace("Ł", "L"))
    text = "".join(c for c in text if not unicodedata.combining(c)).lower()
    return re.sub(r"[^a-z0-9]+", "-", text).strip("-")


def tidy(text):
    text = text.translate(CHARACTER_FIXES)
    text = re.sub(r"[ \t]+", " ", text)
    text = re.sub(r" *\n *", "\n", text)
    text = re.sub(r"\n{2,}", "\n", text)
    return text.strip()


def join_lines(lines):
    result = ""
    for line in lines:
        line = line.strip()
        if not line:
            continue
        if not result:
            result = line
        elif result.endswith("-") and len(result) > 1 and result[-2].isalpha() and LOWER_START.match(line):
            result = result[:-1] + line
        elif result.endswith("-") and line.startswith("-"):
            result = result[:-1] + line
        else:
            result += " " + line
    return result


def split_sentences(text, limit):
    sentences = re.split(r"(?<=[.;:])\s+(?=[A-ZĄĆĘŁŃÓŚŹŻ0-9•−–-])", text)
    return pack(sentences, limit, " ")


def pack(units, limit=MAX_CHUNK, separator="\n"):
    parts = []
    current = []
    size = 0
    for unit in units:
        if len(unit) > limit and separator == "\n":
            if current:
                parts.append(current)
                current, size = [], 0
            parts.extend([s] for s in split_sentences(unit, limit))
            continue
        if current and size + len(unit) > limit:
            parts.append(current)
            current, size = [], 0
        current.append(unit)
        size += len(unit) + 1
    if current:
        parts.append(current)
    if len(parts) > 1 and len(separator.join(parts[-1])) < MIN_CHUNK:
        tail = parts.pop()
        parts[-1] += tail
    return [separator.join(p) for p in parts]


def chunk(chunk_id, source, location, text):
    return {
        "id": chunk_id,
        "source": source,
        "location": location,
        "title": f"{source}, {location}",
        "text": tidy(text),
    }


def numbered_parts(base_id, source, location, units):
    parts = pack(units)
    if len(parts) == 1:
        return [chunk(base_id, source, location, parts[0])]
    return [
        chunk(f"{base_id}-cz{i}", source, f"{location} (część {i}/{len(parts)})", text)
        for i, text in enumerate(parts, 1)
    ]


def pdf_lines(path, min_size, skip_line=None):
    doc = pymupdf.open(path)
    for page_no, page in enumerate(doc, 1):
        for block in page.get_text("dict")["blocks"]:
            lines = []
            for line in block.get("lines", []):
                spans = [s for s in line["spans"] if s["size"] >= min_size and s["text"].strip()]
                text = re.sub(r"[\s" + chr(0xFEFF) + "]+", " ", "".join(s["text"] for s in spans)).strip()
                if not text or (skip_line and skip_line(text)):
                    continue
                bold = all("Bold" in s["font"] for s in spans)
                lines.append({"text": text, "size": max(s["size"] for s in spans), "bold": bold})
            if lines:
                yield page_no, lines


def gazette_header(text):
    return re.fullmatch(r"(Dziennik Ustaw)?\s*(– \d+ –)?\s*(Poz\. \d+)?\s*", text) is not None


def gazette_paragraphs(path):
    return [join_lines(l["text"] for l in lines) for _, lines in pdf_lines(path, 9.5, gazette_header)]


def legal_units(text, unit_marker):
    pieces = re.split(rf"\s(?={unit_marker})", text)
    if len(pieces) > 1 and re.fullmatch(r"(?:Art\.|§) \d+[a-z]*\.", pieces[0].strip()):
        pieces[1] = f"{pieces[0].strip()} {pieces[1]}"
        pieces = pieces[1:]
    return [p for p in pieces if not re.fullmatch(r"\d+[a-z]?\. \(uchylony\)", p.strip())]


def unit_range(units, location):
    numbers = []
    for unit in units:
        m = re.match(r"(?:(?:Art\. \d+[a-z]*\.|§ \d+[a-z]*\.)\s)?(\d+[a-z]?)\. ", unit)
        if m:
            numbers.append(m.group(1))
    if not numbers:
        return location
    if numbers[0] == numbers[-1]:
        return f"{location} ust. {numbers[0]}"
    return f"{location} ust. {numbers[0]}–{numbers[-1]}"


def split_provisions(paragraphs, header, start, end):
    text = "\n".join(paragraphs)
    text = text[text.index(start):]
    if end:
        text = text[:text.index(end)]
    text = re.sub(r"\s*Rozdział \d+[a-z]?\s+[^\n.]*?(?=\s*Art\. \d)", "\n", text)
    pieces = re.split(rf"\s(?={header})", text)
    result = {}
    for piece in pieces:
        m = re.match(header, piece)
        if m:
            result[m.group(1)] = re.sub(r"\s+", " ", piece).strip()
    return result


def provision_chunks(provisions, keys, base_id, source, label, topics=None):
    chunks = []
    ust_marker = r"\d+[a-z]?\. (?:[A-ZĄĆĘŁŃÓŚŹŻ(„]|\(uchylony\))"
    topic = (lambda key: f" – {topics[key]}" if topics and key in topics else "")
    for key in keys:
        text = provisions[key]
        location = f"{label} {key}"
        if len(text) <= MAX_CHUNK:
            chunks.append(chunk(f"{base_id}-{slug(key)}", source, location + topic(key), text))
            continue
        parts = pack(legal_units(text, ust_marker))
        for i, part in enumerate(parts, 1):
            units = legal_units(part, ust_marker)
            chunks.append(chunk(f"{base_id}-{slug(key)}-cz{i}", source, unit_range(units, location) + topic(key), part))
    return chunks


REGULATION_TOPICS = {
    "rozp-okresowe": {
        "1": "zakres rozporządzenia",
        "2": "kryteria obowiązkowe i dodatkowe",
        "3": "arkusze oceny A, B i C",
        "4": "kryteria obowiązkowe (arkusz A, arkusz B, arkusz C)",
        "5": "rozmowa na początku okresu oceny, wybór kryteriów dodatkowych",
        "6": "zapoznanie z kryteriami i terminem oceny",
        "7": "termin ustalenia kryteriów",
        "8": "zmiana terminu oceny",
        "9": "zmiana stanowiska w okresie oceny",
        "10": "rozmowa oceniająca",
        "11": "skala ocen, punkty, ocena pozytywna i negatywna",
        "12": "zapoznanie z oceną, akta osobowe",
        "13": "przepisy przejściowe",
        "14": "wejście w życie",
    },
    "rozp-pierwsza": {
        "1": "zakres rozporządzenia",
        "2": "co bierze się pod uwagę w pierwszej ocenie",
        "3": "termin złożenia sprawozdania",
        "4": "termin sprawozdania i dokonania pierwszej oceny",
        "5": "nowy termin sprawozdania",
        "6": "sprawozdanie ocenianego",
        "7": "na czym polega pierwsza ocena",
        "8": "rozmowa oceniająca",
        "9": "skala punktowa pierwszej oceny",
        "10": "warunki pozytywnej i negatywnej pierwszej oceny",
        "11": "zapoznanie z pierwszą oceną, akta osobowe",
        "12": "przepisy przejściowe",
        "13": "uchylenie poprzedniego rozporządzenia",
        "14": "wejście w życie",
    },
}


def build_act():
    path = SOURCES / "ustawa-o-sluzbie-cywilnej-2026-poz-590.pdf"
    source = "Ustawa o służbie cywilnej"
    provisions = split_provisions(gazette_paragraphs(path), r"Art\. (\d+[a-z]*)\. ", "Rozdział 1", None)
    return provision_chunks(provisions, ACT_ARTICLES, "ustawa-art", source, "art.")


def annex_table_chunks(path, base_id, source, annex_label, kind):
    chunks = []
    doc = pymupdf.open(path)
    for page in doc:
        for table in page.find_tables().tables:
            for row in table.extract():
                cells = [join_lines((c or "").split("\n")) for c in row if c and c.strip()]
                if len(cells) < 2 or not re.match(r"\d+\. [A-ZĄĆĘŁŃÓŚŹŻ]", cells[0]):
                    continue
                name = re.sub(r"^\d+\.\s*", "", cells[0]).strip()
                pretty = name.capitalize().replace("/ ", "/")
                text = f"{kind} {pretty}: {cells[1]}"
                chunks.append(chunk(f"{base_id}-{slug(name)}", source, f"{annex_label}, {kind.lower()} {pretty.lower()}", text))
    return chunks


def build_regulation(file_name, base_id, source, kind):
    path = SOURCES / file_name
    provisions = split_provisions(gazette_paragraphs(path), r"§ (\d+[a-z]*)\. ", "§ 1.", "Prezes Rady Ministrów:")
    keys = sorted(provisions, key=lambda k: int(re.match(r"\d+", k).group()))
    chunks = provision_chunks(provisions, keys, base_id, source, "§", REGULATION_TOPICS.get(base_id))
    chunks += annex_table_chunks(path, base_id, source, "załącznik nr 1", kind)
    return chunks


HEADING = re.compile(r"^(\d+(?:\.\d+)*)\.\s+\S")


def build_guide(file_name, base_id, source):
    path = SOURCES / file_name
    sections = []
    current = None
    previous_heading = False
    for page_no, lines in pdf_lines(path, 9, lambda t: re.fullmatch(r"\d+", t) is not None):
        if page_no < 3:
            continue
        body = []
        for line in lines:
            is_heading_style = line["bold"] and line["size"] >= 11.5
            if is_heading_style and HEADING.match(line["text"]):
                if "Załączniki" in line["text"]:
                    return guide_chunks(sections + ([current] if current else []), base_id, source)
                if current:
                    if body:
                        current["paragraphs"].append(join_lines(body))
                        body = []
                    sections.append(current)
                current = {"heading": line["text"].strip(), "page": page_no, "paragraphs": []}
                previous_heading = True
                continue
            if is_heading_style and previous_heading and current and not current["paragraphs"] and not body:
                current["heading"] = join_lines([current["heading"], line["text"]])
                continue
            previous_heading = False
            body.append(line["text"])
        if current and body:
            current["paragraphs"].append(join_lines(body))
    return guide_chunks(sections + ([current] if current else []), base_id, source)


def guide_chunks(sections, base_id, source):
    chunks = []
    carry = ""
    for section in sections:
        text = "\n".join(section["paragraphs"]).strip()
        if len(text) < MIN_CHUNK:
            carry = join_lines([carry, section["heading"], text])
            continue
        number = HEADING.match(section["heading"]).group(1)
        heading = re.sub(r"\s+", " ", section["heading"]).strip()
        units = ([carry] if carry else []) + section["paragraphs"]
        carry = ""
        chunks += numbered_parts(f"{base_id}-{number.replace('.', '-')}", source, f"rozdz. {heading}, s. {section['page']}", units)
    return chunks


def is_criteria_table(table):
    first_cell = table.find(f"{W}tr/{W}tc")
    text = "" if first_cell is None else "".join(t.text or "" for t in first_cell.iter(W + "t"))
    return text.strip().lower().startswith("kryteria obowiązkowe")


def docx_paragraphs(path):
    archive = zipfile.ZipFile(path)
    root = ET.fromstring(archive.read("word/document.xml"))
    footnotes = {}
    if "word/footnotes.xml" in archive.namelist():
        for note in ET.fromstring(archive.read("word/footnotes.xml")).findall(W + "footnote"):
            text = "".join(t.text or "" for t in note.iter(W + "t")).strip()
            if text:
                footnotes[note.get(W + "id")] = re.sub(r"^\)\s*", "", text)
    paragraphs = []

    def walk(element, buffer):
        if element.tag == MC_FALLBACK:
            return
        if element.tag == W + "tbl" and is_criteria_table(element):
            rows = [row.findall(W + "tc") for row in element.findall(W + "tr")]
            for column in range(max(len(cells) for cells in rows)):
                for cells in rows:
                    if column < len(cells):
                        walk(cells[column], buffer)
            return
        if element.tag == W + "p":
            own = {"text": "", "notes": []}
            for child in element:
                walk(child, own)
            text = re.sub(r"\s+", " ", own["text"]).strip()
            if text:
                paragraphs.append((text, own["notes"]))
            return
        if element.tag == W + "t" and buffer is not None:
            buffer["text"] += element.text or ""
        elif element.tag == W + "tab" and buffer is not None:
            buffer["text"] += " "
        elif element.tag == W + "footnoteReference" and buffer is not None:
            note_id = element.get(W + "id")
            buffer["text"] += note_id
            buffer["notes"].append(note_id)
        for child in element:
            walk(child, buffer)

    walk(root, None)
    return paragraphs, footnotes


FORM_FILLER = re.compile(
    r"^(?:[.…]+|miejscowość|data \(dd\.mm\.rrrr\)|\(dd\.mm\.rrrr\)|\(rrrr\)|podpis \w+|imię i nazwisko lub pieczątka \w+|WZÓR|Wybierz element\.)$"
)


def build_form(file_name, base_id, source, section_starts):
    paragraphs, footnotes = docx_paragraphs(SOURCES / file_name)
    sections = []
    for text, notes in paragraphs:
        if FORM_FILLER.match(text):
            continue
        start = next((label for pattern, label in section_starts if re.match(pattern, text)), None)
        if start and sections and sections[-1]["implicit"]:
            sections[-1]["label"] = start
            sections[-1]["implicit"] = False
        elif start or not sections:
            sections.append({"label": start or section_starts[0][1], "implicit": not start, "lines": [], "notes": []})
        sections[-1]["lines"].append(text)
        sections[-1]["notes"] += notes
    chunks = []
    for section in sections:
        units = list(section["lines"])
        notes = [f"Przypis {n}) {footnotes[n]}" for n in dict.fromkeys(section["notes"]) if n in footnotes]
        if notes:
            units.append("Objaśnienia do tej części arkusza:")
            units += notes
        chunks += numbered_parts(f"{base_id}-{slug(section['label'])}", source, section["label"], units)
    return chunks


def xlsx_rows(archive, sheet_path):
    root = ET.fromstring(archive.read(sheet_path))
    for row in root.iter(X + "row"):
        values = []
        for cell in row.findall(X + "c"):
            inline = cell.find(X + "is")
            value = cell.find(X + "v")
            text = "".join(t.text or "" for t in inline.iter(X + "t")) if inline is not None else (value.text if value is not None else "")
            text = re.sub(r"\s+", " ", text or "").strip()
            if text and (not values or values[-1] != text):
                values.append(text)
        if values:
            yield values


def build_competency_model():
    path = SOURCES / "model-kompetencji.xlsx"
    source = "Model kompetencji"
    archive = zipfile.ZipFile(path)
    workbook = ET.fromstring(archive.read("xl/workbook.xml"))
    sheet_count = len(workbook.find(X + "sheets"))
    chunks = []
    overview = [" | ".join(values) for values in xlsx_rows(archive, "xl/worksheets/sheet1.xml")]
    chunks.append(chunk("model-kompetencji-wprowadzenie", source, "wprowadzenie i lista kompetencji", "\n".join(overview)))
    for index in range(2, sheet_count + 1):
        rows = list(xlsx_rows(archive, f"xl/worksheets/sheet{index}.xml"))
        name = rows[0][0]
        levels = rows[1][1:]
        units = []
        for values in rows[2:]:
            indicator = values[0]
            described = "; ".join(f"{level}: {text}" for level, text in zip(levels, values[1:]))
            units.append(f"Wskaźnik: {indicator} Poziomy: {described}")
        pretty = name.capitalize()
        units[0] = f"Kompetencja {pretty}.\n{units[0]}"
        chunks += numbered_parts(f"model-{slug(name)}", source, f"kompetencja {pretty.lower()}", units)
    return chunks


OSP_GROUPS = [
    ("CZĘŚĆ I", "dane stanowiska i cel"),
    ("GŁÓWNE ZADANIA REALIZOWANE NA STANOWISKU PRACY", "zadania"),
    ("ZŁOŻONOŚĆ I KREATYWNOŚĆ", "charakterystyka pracy"),
    ("WYMAGANE KOMPETENCJE I DOŚWIADCZENIE ZAWODOWE", "wymagania i kompetencje"),
]


def build_osp(file_name, position):
    paragraphs, _ = docx_paragraphs(SOURCES / "osp" / file_name)
    source = f"Przykładowy opis stanowiska pracy (OSP): {position}"
    groups = []
    skipping = False
    for text, _ in paragraphs:
        if text.startswith("ZAPOZNANIE PRACOWNIKA"):
            skipping = True
            continue
        if text.startswith("CZĘŚĆ III"):
            break
        if text.startswith("CZĘŚĆ II"):
            skipping = False
            continue
        if skipping or text.startswith("☐") or FORM_FILLER.match(text) or text in ("/elektroniczna akceptacja/", "OPIS", "(opcjonalnie)"):
            continue
        label = next((l for marker, l in OSP_GROUPS if text.strip() == marker), None)
        if label:
            groups.append({"label": label, "lines": []})
            if label != "dane stanowiska i cel":
                groups[-1]["lines"].append(text)
            continue
        if groups:
            groups[-1]["lines"].append(text.replace("☒", "[x]"))
    chunks = []
    base = f"osp-{slug(position)}"
    for group in groups:
        units = [f"Stanowisko: {position}."] + group["lines"]
        chunks += numbered_parts(f"{base}-{slug(group['label'])}", source, group["label"], units)
    return chunks


def check(chunks):
    ids = [c["id"] for c in chunks]
    duplicates = {i for i in ids if ids.count(i) > 1}
    if duplicates:
        raise SystemExit(f"Duplicate chunk ids: {sorted(duplicates)}")
    empty = [c["id"] for c in chunks if not c["text"] or not c["title"]]
    if empty:
        raise SystemExit(f"Empty chunks: {empty}")


def source_fingerprints():
    result = []
    for path in sorted(SOURCES.rglob("*")):
        if path.is_file():
            digest = hashlib.sha256(path.read_bytes()).hexdigest()[:16]
            result.append({"file": path.relative_to(SOURCES).as_posix(), "sha256": digest})
    return result


def main():
    sys.stdout.reconfigure(encoding="utf-8")
    chunks = []
    chunks += build_act()
    chunks += build_regulation(
        "rozporzadzenie-oceny-okresowe-2016.pdf", "rozp-okresowe", "Rozporządzenie o ocenach okresowych (2016)", "Kryterium")
    chunks += build_regulation(
        "rozporzadzenie-pierwsza-ocena-2015.pdf", "rozp-pierwsza", "Rozporządzenie o pierwszej ocenie (2015)", "Element oceny")
    chunks += build_guide("poradnik-oceny-okresowe.pdf", "poradnik-okresowe", "Poradnik: oceny okresowe w służbie cywilnej")
    chunks += build_guide("poradnik-pierwsza-ocena-2016.pdf", "poradnik-pierwsza", "Poradnik: pierwsza ocena w służbie cywilnej")
    sheet_parts = [(r"^CZĘŚĆ I$", "część I"), (r"^CZĘŚĆ II$", "część II"), (r"^CZĘŚĆ III$", "część III")]
    chunks += build_form("arkusz-oceny-b.docx", "arkusz-b", "Arkusz oceny okresowej B (kierujący zespołem)", sheet_parts)
    chunks += build_form("arkusz-oceny-c.docx", "arkusz-c", "Arkusz oceny okresowej C (niekierujący zespołem)", sheet_parts)
    chunks += build_form("arkusz-pierwszej-oceny.docx", "arkusz-pierwsza", "Arkusz pierwszej oceny", [
        (r"^CZĘŚĆ OGÓLNA", "część ogólna"),
        (r"^ROZMOWA WSTĘPNA", "rozmowa wstępna"),
        (r"^ROZMOWA OCENIAJĄCA", "rozmowa oceniająca"),
        (r"^OCENA POSTAWY", "ocena i wnioski"),
    ])
    chunks += build_competency_model()
    chunks += build_osp("osp-starszy-specjalista.docx", "starszy specjalista")
    chunks += build_osp("osp-radca.docx", "radca")
    chunks += build_osp("osp-naczelnik-wydzialu.docx", "naczelnik wydziału")
    check(chunks)

    data = {"sources": source_fingerprints(), "chunks": chunks}
    OUTPUT.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")

    by_source = {}
    for c in chunks:
        by_source.setdefault(c["source"], []).append(len(c["text"]))
    total = sum(sum(v) for v in by_source.values())
    for source, sizes in by_source.items():
        print(f"{len(sizes):4d} chunks  {sum(sizes):7d} chars  max {max(sizes):5d}  {source}")
    print(f"{len(chunks):4d} chunks  {total:7d} chars  ~{round(total / 3.5):,} tokens  total")


if __name__ == "__main__":
    main()
