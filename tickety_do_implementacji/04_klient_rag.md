# 04. `KnowledgeBaseChatClient`: fragmenty w prompcie i źródła w czacie

Decyzje: D1, D9 (`README.md` w tym folderze). Wymaga ticketu 03.

## Cel

Asystent odpowiada na podstawie fragmentów bazy wiedzy i pokazuje źródła jako chipy pod odpowiedzią.

## Klasa

`Assistant/KnowledgeBaseChatClient : DelegatingChatClient` (Microsoft.Extensions.AI), w łańcuchu:

```
AIChatService (Radzen) → KnowledgeBaseChatClient → klient Claude albo MockChatClient
```

Rejestracja w `AssistantClientFactory` albo w `Program.cs` przez `AsBuilder().Use(...)`. Ważne: działa też z `MockChatClient`, żeby osoby bez klucza widziały ten sam przepływ.

## Przebieg `GetStreamingResponseAsync`

1. **Sztywna odmowa przy pytaniu o poziom oceny.**
   - Przenieść słowa kluczowe i treść odmowy z `MockChatClient` (zmienna `Answers`, pierwszy wpis) do wspólnej klasy, np. `EvaluationLevelGuard`.
   - Jeśli pytanie pasuje, zwrócić odmowę bez wywołania modelu.
   - Zasada z `CLAUDE.md`: AI nigdy nie ustala poziomu oceny.
2. **Wyszukanie fragmentów** (ticket 03): zapytanie z dwóch ostatnich pytań użytkownika, 5 fragmentów.
3. **Dołożenie fragmentów do rozmowy.** Dodać wiadomość systemową (albo dopisać do `ChatOptions.Instructions`) z ponumerowanymi fragmentami:
   ```
   [1] Ustawa o służbie cywilnej, art. 83
   <tekst>

   [2] ...
   ```
   Nie modyfikować historii trzymanej przez Radzena, tylko kopię listy wiadomości przekazywaną dalej.
4. **Wywołanie modelu** (`base.GetStreamingResponseAsync`) i przekazywanie aktualizacji strumienia dalej bez zmian.
5. **Źródła po zakończeniu strumienia.** Ostatnia aktualizacja z pustym `TextContent` i `Annotations` = `CitationAnnotation { Title, Snippet }` dla fragmentów, tak jak robi to dziś `MockChatClient`.
   - Pokazywać tylko fragmenty, które model faktycznie przywołał numerem `[n]` w odpowiedzi; jeśli żadnego nie przywołał, nie pokazywać źródeł.
   - `Snippet` = początek tekstu fragmentu (np. 300 znaków).

## Prompt systemowy (`AssistantChat.razor`, stała `SystemPrompt`)

Przywrócić wersję „tylko z fragmentów” (wariant B, D9):

- odpowiadaj po polsku, zwięźle, wyłącznie na podstawie fragmentów bazy wiedzy z tej rozmowy;
- przy każdej informacji podawaj numer fragmentu w nawiasie kwadratowym, np. [1];
- jeśli fragmenty nie odpowiadają na pytanie o zasady oceny, napisz „Nie znalazłem tego w bazie wiedzy.” i zaproponuj sprawdzenie w przepisach albo w komórce kadrowej;
- przy prośbach o przygotowanie rozmowy (pytania, argumenty, plan rozwoju) możesz korzystać z wiedzy ogólnej, ale nie podawaj wtedy przepisów, których nie ma we fragmentach;
- nigdy nie proponuj ani nie oceniaj poziomu oceny pracownika;
- nie odpowiadaj na pytania niezwiązane z oceną w służbie cywilnej.

## Do sprawdzenia przy implementacji

- Czy `ChatOptions.Instructions` dochodzi do Claude jako prompt systemowy (w logach `Trace` widać `instructions`) i czy dodatkowa wiadomość systemowa w liście nie jest odrzucana przez API (Claude przyjmuje `system` na górze; wiadomości systemowe w środku rozmowy mają swoje reguły). Najbezpieczniej doklejać fragmenty do `Instructions`.
- Etykieta „Sources” nad chipami jest po angielsku. Rozważyć `Radzen.ILocalizer` z polskim tekstem („Źródła”) albo `CitationTemplate`/`MessageFooterTemplate` w `RadzenAIChat`.
- `MockChatClient` ma własne źródła na sztywno. Po wprowadzeniu RAG uprościć go: niech odpowiada na podstawie przekazanych fragmentów (np. zwraca pierwsze zdanie fragmentu [1]) albo zostaje jak jest. Decyzja przy implementacji.

## Weryfikacja ręczna (z kluczem API)

- „Ile dni ma pracownik na sprzeciw od oceny okresowej?” → 7 dni, źródło: ustawa, art. 83.
- „Jakie kryteria są obowiązkowe w arkuszu C?” → lista kryteriów, źródło: arkusz C lub rozporządzenie.
- „A od kiedy liczy się ten termin?” (dopytanie) → od dnia zapoznania się z oceną.
- „Jaki poziom dać Annie za współpracę?” → odmowa bez wywołania modelu (w logach brak `GetStreamingResponseAsync invoked` do Claude).
- „Jaka jest stolica Francji?” → odmowa tematu.
- Po testach zatrzymać aplikację i zamknąć kartę.
