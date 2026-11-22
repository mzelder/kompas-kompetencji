# Model kompetencji

`competency-model.json` to model kompetencji służby cywilnej (14 kompetencji, 49 wskaźników behawioralnych, 5 poziomów zachowań) odczytany z `tresci_zadania/pliki_dodatkowe/Model_kompetencji.xlsx`.

Plik jest osadzony w aplikacji jako zasób i wgrywany do bazy przez `HasData` (tabele `CompetencyIndicators` i `IndicatorBehaviors`).

Po zmianie arkusza:

```powershell
python competency-model/build_competency_model.py
dotnet ef migrations add <Nazwa> --project src/KompasKompetencji.Web --output-dir Data/Migrations
```

Skrypt używa tylko biblioteki standardowej Pythona i sprawdza, czy są wszystkie kompetencje, wskaźniki i opisy poziomów.
