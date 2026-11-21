# 07. Przełącznik dostawcy AI (opcjonalny)

Decyzja: D13 (`README.md` w tym folderze) i sekcja „Dostawca modelu AI a wymóg »darmowych narzędzi AI«” w `do_rozwazenia.md`.

**Realizować dopiero, gdy** organizatorzy odpowiedzą, że płatne Claude API (także na darmowych kredytach startowych) nie spełnia wymogu „darmowych narzędzi AI”, albo gdy zespół zechce mieć wariant w pełni lokalny na demo.

## Cel

Wybór dostawcy w konfiguracji, bez zmian w kodzie: `Assistant:Provider` = `Anthropic` albo `Ollama`.

## Zakres

- `AssistantClientFactory`:
  - `Anthropic`: obecne zachowanie (`AnthropicClient.AsIChatClient`, zerowanie `Temperature`, `MaxOutputTokens`);
  - `Ollama`: klient do lokalnej Ollamy (np. `http://localhost:11434`), model z `Assistant:Model` (np. Bielik).
  - Przed wyborem pakietu sprawdzić **w aktualnej dokumentacji**, czym najlepiej podpiąć Ollamę do `IChatClient`: pakiet dedykowany albo endpoint zgodny z OpenAI (`/v1`) przez `Microsoft.Extensions.AI.OpenAI`. Nie zgadywać z pamięci: GitHub Models okazał się wyłączony, a rekomendacja była nieaktualna.
  - Bez klucza i bez dostawcy: `MockChatClient` jak dziś.
- `appsettings.json`: `Provider`, `Model`, dla Ollamy `Endpoint`.
- `README.md`: jak zainstalować Ollamę, pobrać model i przełączyć dostawcę.
- Sprawdzić jakość odpowiedzi na zestawie pytań z ticketu 05 (ręcznie, bo testy automatyczne nie wołają modelu) i porównać z Claude.

## Ryzyka

- Lokalny model potrzebuje mocnego komputera (GPU albo Mac z procesorem M) i odpowiada wolniej.
- Mniejsze okno kontekstu niż Claude, ale RAG wysyła tylko 5 fragmentów, więc nie powinno to przeszkadzać.
