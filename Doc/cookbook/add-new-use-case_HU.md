# Új Use Case hozzáadása

[🇬🇧 English version](add-new-use-case.md) · [🇭🇺 Receptgyűjtemény](README_HU.md) · [🇭🇺 Architektúra kézikönyv](../architecture_HU.md)

Use-case contractot és implementációt akkor helyezz az Application rétegbe, ha az operation workflow-t, validationt vagy contractokon keresztüli persistence-t koordinál. A Presentation egy ViewModelből hívja a contractot. Ha persistence szükséges, definiálj vagy használj újra Application repository/service contractot, és az implementációt tedd az Infrastructure rétegbe. Regisztráld a use case-et a `BindApplication()` metódusban. Adj hozzá Application teszteket stub contractokkal, valamint Presentation teszteket a ViewModel viselkedéshez.

Kapcsolódó oldal: [🇭🇺 Architektúra kézikönyv](../architecture_HU.md).
