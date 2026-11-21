❯

# Kompas kompetencji: podsumowanie zadania

Źródło: `tresci_zadania/Pełna treść zadania konkursowego.pdf` (wyzwanie Ministerstwa Cyfryzacji)

## 1. Opis zadania

Trzeba zbudować działający prototyp aplikacji webowej „Kompas kompetencji”. Ma ona prowadzić przełożonego w służbie cywilnej krok po kroku przez ustawowy proces oceny pracownika: pierwszą ocenę albo ocenę okresową. Dziś przełożony musi sam połączyć przepisy, formularze, model kompetencyjny i poradniki. Najtrudniej jest przełożyć obserwowane zachowania pracownika na poziom spełniania kompetencji, a to prowadzi do niespójnych ocen.

Narzędzie ma zebrać to wszystko w jednym miejscu. Kompetencje ocenia się przez wybór konkretnych, obserwowalnych zachowań (wskaźników behawioralnych), a system na tej podstawie podpowiada poziom kompetencji. Ostateczna decyzja zawsze należy do przełożonego. Integralną częścią jest asystent AI oparty na bazie wiedzy (przepisy, model kompetencyjny, poradniki KPRM). Wyjaśnia on zasady formalne i pomaga przygotować się do rozmowy z pracownikiem. Dodatkowo HR/administrator dostaje podgląd statusów, terminów i przypomnień.

## 2. Funkcjonalności

### 2.1 Użytkownicy, role i dostęp
- **Przełożony** (główny użytkownik) przeprowadza ocenę swoich pracowników.
- **HR / administrator** (użytkownik dodatkowy) organizuje i monitoruje proces.
- Uwierzytelnianie, autoryzacja oraz zarządzanie rolami i uprawnieniami. Przełożony widzi tylko swoich pracowników.

### 2.2 Dwie odrębne ścieżki oceny (kreator krok po kroku)
- **Pierwsza ocena w służbie cywilnej** oraz **ocena okresowa**. Każda ma własne etapy i zasady wynikające z właściwego rozporządzenia.
- Kreator (wizard) prowadzi przez kolejne etapy: rozpoczęcie procesu, dobór kryteriów, ocena kompetencji, podsumowanie i decyzja, rozmowa, dokumentacja.
- Każda ścieżka wypełnia **właściwą część właściwego formularza** (arkusz pierwszej oceny albo arkusz oceny okresowej B/C).
- Widoczny postęp procesu i możliwość powrotu do niedokończonej oceny.

### 2.3 Dobór kryteriów oceny
- Wsparcie w wyborze kryteriów właściwych dla danego pracownika i stanowiska, np. na podstawie opisu stanowiska pracy (OSP).
- Kontrola zgodności wyboru z wymaganiami formalnymi, np. liczba i rodzaj kryteriów obowiązkowych i do wyboru.

### 2.4 Model kompetencyjny i ocena behawioralna (rdzeń rozwiązania)
- Wbudowany model kompetencyjny: kompetencje właściwe dla danego rodzaju oceny, ich **wskaźniki behawioralne** i **opisy poziomów spełniania**.
- Dla każdego wskaźnika przełożony wybiera **zachowanie, które najlepiej opisuje to, co obserwował** u pracownika.
- System **oblicza i pokazuje sugerowany poziom** spełniania kompetencji na podstawie tych wyborów.
- **Ostateczny poziom zawsze wybiera przełożony.** Może przyjąć sugestię albo ją zmienić (warto wtedy poprosić o uzasadnienie).
- Miejsce na notatki i przykłady zachowań, które potem posłużą do uzasadnienia oceny i do rozmowy.

### 2.5 Dokumentacja
- Generowanie wymaganej dokumentacji na podstawie oficjalnych formularzy, wypełnionej danymi z procesu.
- **Eksport do PDF** (wymaganie wprost).

### 2.6 Terminy i komunikacja
- Zarządzanie terminami poszczególnych etapów oceny.
- Komunikacja z przełożonymi, np. powiadomienia o zbliżających się lub przekroczonych terminach.

### 2.7 Asystent AI (czat)
- **Obszar 1, wymagania formalne:** odpowiada na pytania o zasady pierwszej i okresowej oceny oraz o przebieg procesu.
- **Obszar 2, przygotowanie do rozmowy:** proponuje pytania do pracownika, wskazuje kwestie do omówienia i pomaga uporządkować argumenty uzasadniające ocenę.
- **Baza wiedzy (RAG):** ustawa o służbie cywilnej, rozporządzenia o pierwszej i okresowej ocenie, model kompetencyjny, poradniki i materiały KPRM oraz przygotowany zestaw pytań i odpowiedzi.
- Odpowiada **przede wszystkim na podstawie bazy wiedzy** i **wskazuje źródło** odpowiedzi.
- Gdy w bazie brakuje informacji, **mówi to wprost** zamiast zgadywać.
- **Tylko rola doradcza.** Nie podejmuje decyzji o poziomie oceny.

### 2.8 Panel HR / administratora
- Monitorowanie **statusów** wszystkich procesów oceny.
- Podgląd **terminów** realizacji i zaległości.
- **Wysyłanie przypomnień** o czynnościach do wykonania.

### 2.9 Wymagania niefunkcjonalne
- Aplikacja przeglądarkowa z czytelnym, responsywnym interfejsem, **ale nie dla smartfonów** (docelowo desktop i laptop).
- Architektura gotowa na rozwój i skalowanie. **Udokumentowane API** do bezpiecznej wymiany danych.
- Przygotowanie pod przyszłą integrację z **EZD** i systemami kadrowymi (np. **QNT**). Same integracje nie są wymagane w prototypie.
- **Ochrona danych osobowych**, poufność danych pracowników i zasada minimalizacji danych.
- W prototypie wolno używać tylko **darmowych narzędzi AI**. Trzeba wskazać elementy, które przed wdrożeniem produkcyjnym wymagają dodatkowego zabezpieczenia.

### 2.10 Minimum, które musi zadziałać na demo
Jury ocenia działający prototyp, a nie makiety. Na prezentacji trzeba pokazać:
1. **Jedną kompletną ścieżkę przełożonego:** rozpoczęcie procesu, praca z kryteriami i kompetencjami, ocena wskaźników behawioralnych, sugerowany poziom kompetencji i ostateczna decyzja przełożonego.
2. **Działającego asystenta AI.** Jurorzy będą zadawać pytania o zasady pierwszej i okresowej oceny i sprawdzać poprawność odpowiedzi.

Oprócz prototypu trzeba oddać: krótki opis projektu, prezentację (maks. 10 slajdów), opis wykorzystania AI oraz informację o technologiach i architekturze.

Wagi kryteriów oceny:

| Kryterium | Waga |
|---|---|
| Zgodność merytoryczna i realizacja założeń | 25% |
| Użyteczność i UX | 25% |
| Jakość i użyteczność AI | 20% |
| Innowacyjność i jakość rozwiązania | 20% |
| Potencjał wdrożeniowy | 10% |
