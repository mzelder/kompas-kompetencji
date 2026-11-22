import json
import re
import sys
import unicodedata
import xml.etree.ElementTree as ET
import zipfile
from pathlib import Path

ROOT = Path(__file__).resolve().parent
SOURCE = ROOT.parent / "tresci_zadania" / "pliki_dodatkowe" / "Model_kompetencji.xlsx"
OUTPUT = ROOT / "competency-model.json"

NS = {"m": "http://schemas.openxmlformats.org/spreadsheetml/2006/main"}
REL_NS = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"

CRITERIA = [
    ("RZETELNOSC", "Reliability"),
    ("KOMUNIKACJA", "Communication"),
    ("WYKORZYSTYWANIE WIEDZY", "KnowledgeAndDevelopment"),
    ("ORGANIZACJA PRACY", "WorkOrganization"),
    ("KREATYWNOSC", "Creativity"),
    ("WSPOLPRACA", "Cooperation"),
    ("MYSLENIE ANALITYCZNE", "AnalyticalThinking"),
    ("NEGOCJOWANIE", "Negotiation"),
    ("WYSTAPIENIA PUBLICZNE", "PublicSpeaking"),
    ("ORIENTACJA NA KLIENTA", "CustomerOrientation"),
    ("RADZENIE SOBIE", "CrisisManagement"),
    ("ZARZADZANIE STRATEGICZNE", "StrategicManagement"),
    ("ZARZADZANIE LUDZMI", "PeopleManagement"),
    ("PODEJMOWANIE DECYZJI", "DecisionMaking"),
]

LEVEL_COLUMNS = ["B", "C", "D", "E", "F"]
EXPECTED_COMPETENCIES = 14
EXPECTED_INDICATORS = 49


def normalize(text):
    decomposed = unicodedata.normalize("NFD", text.upper().replace("Ł", "L"))
    return "".join(c for c in decomposed if unicodedata.category(c) != "Mn")


def clean(text):
    return re.sub(r"\s+", " ", text).strip()


def read_shared_strings(archive):
    if "xl/sharedStrings.xml" not in archive.namelist():
        return []
    root = ET.fromstring(archive.read("xl/sharedStrings.xml"))
    return ["".join(t.text or "" for t in si.iter(f"{{{NS['m']}}}t")) for si in root.findall("m:si", NS)]


def sheet_paths(archive):
    workbook = ET.fromstring(archive.read("xl/workbook.xml"))
    rels = ET.fromstring(archive.read("xl/_rels/workbook.xml.rels"))
    targets = {r.get("Id"): r.get("Target") for r in rels}
    for sheet in workbook.find("m:sheets", NS):
        target = targets[sheet.get(f"{{{REL_NS}}}id")]
        yield sheet.get("name"), "xl/" + target.lstrip("/").removeprefix("xl/")


def read_rows(archive, path, shared):
    root = ET.fromstring(archive.read(path))
    rows = {}
    for row in root.find("m:sheetData", NS):
        values = {}
        for cell in row:
            column = re.match(r"[A-Z]+", cell.get("r")).group()
            value = cell.find("m:v", NS)
            if value is not None:
                values[column] = shared[int(value.text)] if cell.get("t") == "s" else value.text
            elif cell.get("t") == "inlineStr":
                values[column] = "".join(t.text or "" for t in cell.iter(f"{{{NS['m']}}}t"))
        if values:
            rows[int(row.get("r"))] = values
    return rows


def criterion_for(title):
    normalized = normalize(title)
    matches = [code for prefix, code in CRITERIA if normalized.startswith(prefix)]
    if len(matches) != 1:
        raise ValueError(f"Nie rozpoznano kompetencji: {title!r}")
    return matches[0]


def main():
    archive = zipfile.ZipFile(SOURCE)
    shared = read_shared_strings(archive)
    competencies = []
    level_names = None

    for _, path in list(sheet_paths(archive))[1:]:
        rows = read_rows(archive, path, shared)
        title = clean(rows[1]["A"])
        header = rows[3]
        names = [clean(header[c]) for c in LEVEL_COLUMNS]
        if level_names is None:
            level_names = names
        elif names != level_names:
            raise ValueError(f"Inne nazwy poziomów w arkuszu {title!r}")

        indicators = []
        for number in sorted(n for n in rows if n > 3):
            row = rows[number]
            if not row.get("A"):
                continue
            levels = [clean(row.get(c, "")) for c in LEVEL_COLUMNS]
            if not all(levels):
                raise ValueError(f"Brakuje opisu poziomu w {title!r}, wiersz {number}")
            indicators.append({"text": clean(row["A"]), "levels": levels})

        competencies.append({"criterion": criterion_for(title), "name": title, "indicators": indicators})

    indicator_count = sum(len(c["indicators"]) for c in competencies)
    if len(competencies) != EXPECTED_COMPETENCIES or indicator_count != EXPECTED_INDICATORS:
        raise ValueError(f"Oczekiwano {EXPECTED_COMPETENCIES} kompetencji i {EXPECTED_INDICATORS} wskaźników, "
                         f"jest {len(competencies)} i {indicator_count}")
    if len({c["criterion"] for c in competencies}) != EXPECTED_COMPETENCIES:
        raise ValueError("Ta sama kompetencja występuje w dwóch arkuszach")

    data = {"source": SOURCE.name, "levels": level_names, "competencies": competencies}
    OUTPUT.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(f"Zapisano {OUTPUT.name}: {len(competencies)} kompetencji, {indicator_count} wskaźników.")


if __name__ == "__main__":
    sys.exit(main())
