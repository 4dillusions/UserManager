# Új modul hozzáadása

[🇬🇧 English version](add-new-module.md) · [🇭🇺 Receptgyűjtemény](README_HU.md) · [🇭🇺 Architektúra kézikönyv](../architecture_HU.md)

Csak akkor hozz létre új modult, ha valódi ownership vagy boundary indokolja. Adj a modulnak szűk registration entry pointot, tartsd a dependencyket összhangban az architektúrával, és a WPFUI rétegből integráld. Ne hozz létre modult pusztán azért, hogy egy kis folder struktúrája szimmetrikusabbnak tűnjön. Adj hozzá teszteket a viselkedésre és a dependency registrationre.

Kapcsolódó oldal: [🇭🇺 Architektúra kézikönyv](../architecture_HU.md).
