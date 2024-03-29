/* Copyright (c) Business Objects 2006. All rights reserved. */

var L_bobj_crv_MainReport = "Raport g\u0142\u00F3wny";
// Viewer Toolbar tooltips
var L_bobj_crv_FirstPage = "Id\u017A do pierwszej strony";
var L_bobj_crv_PrevPage = "Id\u017A do poprzedniej strony";
var L_bobj_crv_NextPage = "Id\u017A do nast\u0119pnej strony";
var L_bobj_crv_LastPage = "Id\u017A do ostatniej strony";
var L_bobj_crv_ParamPanel = "Panel parametr\u00F3w";
var L_bobj_crv_Parameters = "Parametry";
var L_bobj_crv_GroupTree = "Drzewo grup";
var L_bobj_crv_DrillUp = "Agreguj";
var L_bobj_crv_Refresh = "Od\u015Bwie\u017C raport";
var L_bobj_crv_Zoom = "Powi\u0119ksz";
var L_bobj_crv_PageNav = "Nawigacja strony";
var L_bobj_crv_SelectPage = "Id\u017A do strony";
var L_bobj_crv_SearchText = "Szukaj tekstu";
var L_bobj_crv_Export = "Eksportuj ten raport";
var L_bobj_crv_Print = "Drukuj ten raport";
var L_bobj_crv_TabList = "Lista kart";
var L_bobj_crv_Close = "Zamknij";
var L_bobj_crv_Logo=  "Logo Business Objects";
var L_bobj_crv_FileMenu = "Menu Plik";

var L_bobj_crv_File = "Plik";

var L_bobj_crv_Show = "Poka\u017C";
var L_bobj_crv_Hide = "Ukryj";

var L_bobj_crv_Find = "Znajd\u017A...";
var L_bobj_crv_of = "%1 z %2"; // Example: Page "1 of 3"

var L_bobj_crv_submitBtnLbl = "Eksportuj";
var L_bobj_crv_ActiveXPrintDialogTitle = "Drukuj";
var L_bobj_crv_PDFPrintDialogTitle = "Drukuj do pliku PDF";
var L_bobj_crv_PrintRangeLbl = "Zakres stron:";
var L_bobj_crv_PrintAllLbl = "Wszystkie strony";
var L_bobj_crv_PrintPagesLbl = "Wybierz strony";
var L_bobj_crv_PrintFromLbl = "Od:";
var L_bobj_crv_PrintToLbl = "Do:";
var L_bobj_crv_PrintInfoTitle = "Drukuj do pliku PDF:";
var L_bobj_crv_PrintInfo1 = 'Drukowanie wymaga, aby podgl\u0105d zosta\u0142 wyeksportowany do pliku PDF. Otw\u00F3rz dokument w aplikacji odczytuj\u0105cej pliki PDF i wybierz opcj\u0119 drukowania.';
var L_bobj_crv_PrintInfo2 = 'Uwaga: Do drukowania jest potrzebny program odczytuj\u0105cy pliki PDF (np. Adobe Reader).';
var L_bobj_crv_PrintPageRangeError = "Wprowad\u017A prawid\u0142owy zakres stron.";

var L_bobj_crv_ExportBtnLbl = "Eksportuj";
var L_bobj_crv_ExportDialogTitle = "Eksportuj";
var L_bobj_crv_ExportFormatLbl = "Format pliku:";
var L_bobj_crv_ExportInfoTitle = "Aby wyeksportowa\u0107:";

var L_bobj_crv_ParamsApply = "Zastosuj";
var L_bobj_crv_ParamsAdvDlg = "Edytuj warto\u015B\u0107 parametru";
var L_bobj_crv_ParamsDeleteTooltip = "Usu\u0144 warto\u015B\u0107 parametru";
var L_bobj_crv_ParamsAddValue = "Kliknij, aby doda\u0107...";
var L_bobj_crv_ParamsApplyTip = "Przycisk Zastosuj (w\u0142\u0105czony)";
var L_bobj_crv_ParamsApplyDisabledTip = "Przycisk Zastosuj (wy\u0142\u0105czony)";
var L_bobj_crv_ParamsDlgTitle = "Wprowad\u017A warto\u015Bci";
var L_bobj_crv_ParamsCalBtn = "Przycisk Kalendarz";
var L_bobj_crv_Reset= "Resetuj";
var L_bobj_crv_ResetTip = "Przycisk Resetuj (w\u0142\u0105czony)";
var L_bobj_crv_ResetDisabledTip = "Przycisk Resetuj (wy\u0142\u0105czony)";
var L_bobj_crv_ParamsDirtyTip = "Warto\u015B\u0107 parametru zosta\u0142a zmieniona. Kliknij przycisk Zastosuj, aby zastosowa\u0107 zmiany.";
var L_bobj_crv_ParamsDataTip = "To jest parametr pobieraj\u0105cy dane";
var L_bobj_crv_ParamsMaxNumDefaultValues = "Kliknij tutaj, aby wy\u015Bwietli\u0107 wi\u0119cej element\u00F3w...";
var L_bobj_crv_paramsOpenAdvance = "Przycisk monitu zaawansowanego dla \u201E%1\u201D";

var L_bobj_crv_ParamsInvalidTitle = "Warto\u015B\u0107 parametru jest nieprawid\u0142owa";
var L_bobj_crv_ParamsTooLong = "Warto\u015B\u0107 parametru nie mo\u017Ce zawiera\u0107 wi\u0119cej znak\u00F3w ni\u017C %1";
var L_bobj_crv_ParamsTooShort = "Warto\u015B\u0107 parametru musi zawiera\u0107 przynajmniej %1 znak\u00F3w";
var L_bobj_crv_ParamsBadNumber = "Ten parametr jest typu \\u201ELiczba\\u201D i mo\u017Ce zawiera\u0107 wy\u0142\u0105cznie symbol znaku minus, cyfry (\"0-9\"), znaki separator\u00F3w grupowania cyfr oraz znak separatora dziesi\u0119tnego.";
var L_bobj_crv_ParamsBadCurrency = "Ten parametr jest typu \\u201EWaluta\\u201D i mo\u017Ce zawiera\u0107 wy\u0142\u0105cznie symbol znaku minus, cyfry (\"0-9\"), znaki separator\u00F3w grupowania cyfr oraz znak separatora dziesi\u0119tnego.";
var L_bobj_crv_ParamsBadDate = "Ten parametr jest typu \u201eData\u201d i powinien mie\u0107 format \u201eDate(rrrr,mm,dd)\u201d, gdzie \u201errrr\u201d to rok w formacie czterocyfrowym, \u201emm\u201d \u2014 miesi\u0105c (np. stycze\u0144 = 1), a \u201edd\u201d \u2014 dzie\u0144 miesi\u0105ca.";
var L_bobj_crv_ParamsBadTime = "Ten parametr jest typu \\u201EGodzina\\u201D i jego poprawny format to \\u201ETime(gg,mm,ss)\\u201D, gdzie \\u201Egg\\u201D to godziny wg zegara 24-godzinnego, \\u201Emm\\u201D \u2014 minuty, a \\u201Ess\\u201D \u2014 sekundy.";
var L_bobj_crv_ParamsBadDateTime = "Ten parametr jest typu \u201eData/godzina\u201d i jego poprawny format to \u201eDateTime(rrrr,mm,dd,gg,mm,ss)\u201d, gdzie \u201errrr\u201d to rok w formacie czterocyfrowym, \u201emm\u201d \u2014 miesi\u0105c (np. stycze\u0144 = 1), \u201edd\u201d \u2014 dzie\u0144 miesi\u0105ca, \u201egg\u201d \u2014 godziny wg zegara 24-godzinnego, \u201emm\u201d \u2014 minuty, a \u201ess\u201d \u2014 sekundy.";
var L_bobj_crv_ParamsMinTooltip = "Okre\u015Bl warto\u015B\u0107 %1 wi\u0119ksz\u0105 lub r\u00F3wn\u0105 %2.";
var L_bobj_crv_ParamsMaxTooltip = "Okre\u015Bl warto\u015B\u0107 %1 mniejsz\u0105 lub r\u00F3wn\u0105 %2.";
var L_bobj_crv_ParamsMinAndMaxTooltip = "Okre\u015Bl warto\u015B\u0107 %1 z zakresu od %2 do %3.";
var L_bobj_crv_ParamsStringMinOrMaxTooltip = "D\u0142ugo\u015B\u0107 %1 dla tego pola wynosi %2.";
var L_bobj_crv_ParamsStringMinAndMaxTooltip = "Warto\u015B\u0107 musi mie\u0107 d\u0142ugo\u015B\u0107 od %1 do %2 znak\u00F3w.";
var L_bobj_crv_ParamsYearToken = "rrrr";
var L_bobj_crv_ParamsMonthToken = "mm";
var L_bobj_crv_ParamsDayToken = "dd";
var L_bobj_crv_ParamsReadOnly = "Ten parametr jest typu \\u201ETylko do odczytu\\u201D.";
var L_bobj_crv_ParamsNoValue = "Brak warto\u015Bci";
var L_bobj_crv_ParamsDuplicateValue = "Warto\u015Bci zduplikowane s\u0105 niedozwolone.";
var L_bobj_crv_ParamsEnterOptional = "Wprowad\u017A %1 (opcjonalnie)";
var L_bobj_crv_ParamsNoneSelected= "(Nie wybrano)";
var L_bobj_crv_ParamsClearValues= "Wyczy\u015B\u0107 warto\u015Bci";
var L_bobj_crv_ParamsMoreValues= "Liczba innych warto\u015Bci: %1...";
var L_bobj_crv_ParamsMoreValue= "Liczba innych warto\u015Bci: %1...";
var L_bobj_crv_Error = "B\u0142\u0105d";
var L_bobj_crv_OK = "OK";
var L_bobj_crv_Cancel = "Anuluj";
var L_bobj_crv_showDetails = "Poka\u017C szczeg\u00F3\u0142y";
var L_bobj_crv_hideDetails = "Ukryj szczeg\u00F3\u0142y";
var L_bobj_crv_RequestError = "Nie mo\u017Cna przetworzy\u0107 \u017C\u0105dania";
var L_bobj_crv_ServletMissing = "Podgl\u0105d nie mo\u017Ce po\u0142\u0105czy\u0107 si\u0119 z serwletem CrystalReportViewerServlet, kt\u00F3ry obs\u0142uguje \u017C\u0105dania asynchroniczne.\nUpewnij si\u0119, \u017Ce serwlet i mapowanie serwletu poprawnie zadeklarowano w pliku xml aplikacji sieci Web.";
var L_bobj_crv_FlashRequired = "Ta zawarto\u015B\u0107 wymaga programu Adobe Flash Player 9 lub nowszego. {0}Kliknij tutaj, aby go zainstalowa\u0107";
var L_bobj_crv_ReadOnlyInPanel= "Tego parametru nie mo\u017Cna edytowa\u0107 w panelu. Otw\u00F3rz okno dialogowe monitu zaawansowanego, aby zmodyfikowa\u0107 warto\u015B\u0107 parametru";

var L_bobj_crv_Tree_Drilldown_Node = "W\u0119ze\u0142 dr\u0105\u017Cenia danych %1";

var L_bobj_crv_ReportProcessingMessage = "Trwa przetwarzanie dokumentu. Czekaj";
var L_bobj_crv_PrintControlProcessingMessage = "Zaczekaj, a\u017C zostanie za\u0142adowany formant drukowania programu Crystal Reports.";

var L_bobj_crv_SundayShort = "Ni";
var L_bobj_crv_MondayShort = "Pn";
var L_bobj_crv_TuesdayShort = "Wt";
var L_bobj_crv_WednesdayShort = "\u015Ar";
var L_bobj_crv_ThursdayShort = "Cz";
var L_bobj_crv_FridayShort = "Pt";
var L_bobj_crv_SaturdayShort = "So";

var L_bobj_crv_Minimum = "Minimum";
var L_bobj_crv_Maximum = "Maksimum";

var L_bobj_crv_Date = "Data";
var L_bobj_crv_Time = "Godzina";
var L_bobj_crv_DateTime = "Data i godzina";
var L_bobj_crv_Boolean = "Logiczna";
var L_bobj_crv_Number = "Liczba";
var L_bobj_crv_Text = "Tekst";

var L_bobj_crv_InteractiveParam_NoAjax = "U\u017Cywana przegl\u0105darka sieci Web nie jest skonfigurowana do pokazywania panelu parametr\u00F3w.";
var L_bobj_crv_AdvancedDialog_NoAjax= "Przegl\u0105darka nie mo\u017Ce otworzy\u0107 okna dialogowego monitu zaawansowanego.";

var L_bobj_crv_EnableAjax= "Aby w\u0142\u0105czy\u0107 obs\u0142ug\u0119 \u017C\u0105da\u0144 asynchronicznych, skontaktuj si\u0119 z administratorem.";

var L_bobj_crv_LastRefreshed = "Ostatnie od\u015Bwie\u017Cenie";

var L_bobj_crv_Collapse = "Zwi\u0144";

var L_bobj_crv_CatalystTip = "Zasoby online";
// <script>
/*
=============================================================
WebIntelligence(r) Report Panel
Copyright(c) 2001-2003 Business Objects S.A.
All rights reserved

Use and support of this software is governed by the terms
and conditions of the software license agreement and support
policy of Business Objects S.A. and/or its subsidiaries.
The Business Objects products and technology are protected
by the US patent number 5,555,403 and 6,247,008

File: labels.js

=============================================================
*/

_default="Domyślny"
_black="Czarny"
_brown="Brązowy"
_oliveGreen="Oliwkowozielony"
_darkGreen="Ciemnozielony"
_darkTeal="Ciemnozielonomodry"
_navyBlue="Granatowy"
_indigo="Indygo"
_darkGray="Ciemnoszary"
_darkRed="Ciemnoczerwony"
_orange="Pomarańczowy"
_darkYellow="Ciemnożółty"
_green="Zielony"
_teal="Zielonomodry"
_blue="Niebieski"
_blueGray="Niebieskoszary"
_mediumGray="Średnioszary"
_red="Czerwony"
_lightOrange="Jasnopomarańczowy"
_lime="Limonowy"
_seaGreen="Morska zieleń"
_aqua="Akwamaryna"
_lightBlue="Jasnoniebieski"
_violet="Fioletowy"
_gray="Szary"
_magenta="Amarantowy"
_gold="Złoty"
_yellow="Żółty"
_brightGreen="Intensywny zielony"
_cyan="Błękitny"
_skyBlue="Lazurowy"
_plum="Śliwkowy"
_lightGray="Jasnoszary"
_pink="Różowy"
_tan="Pastelowobrązowy"
_lightYellow="Jasnożółty"
_lightGreen="Jasnozielony"
_lightTurquoise="Jasnoturkusowy"
_paleBlue="Bladoniebieski"
_lavender="Lawendowy"
_white="Biały"
_lastUsed="Ostatnio używany:"
_moreColors="Więcej kolorów..."

_month=new Array

_month[0]="Styczeń"
_month[1]="Luty"
_month[2]="Marzec"
_month[3]="Kwiecień"
_month[4]="Maj"
_month[5]="Czerwiec"
_month[6]="Lipiec"
_month[7]="Sierpień"
_month[8]="Wrzesień"
_month[9]="Październik"
_month[10]="Listopad"
_month[11]="Grudzień"

_day=new Array
_day[0]="Ni"
_day[1]="Pn"
_day[2]="Wt"
_day[3]="Śr"
_day[4]="Cz"
_day[5]="Pt"
_day[6]="So"

_today="Dziś"

_AM="AM"
_PM="PM"

_closeDialog="Zamknij okno"

_lstMoveUpLab="Przenieś w górę"
_lstMoveDownLab="Przenieś w dół"
_lstMoveLeftLab="Przenieś w lewo"
_lstMoveRightLab="Przenieś w prawo"
_lstNewNodeLab="Dodaj filtr zagnieżdżony"
_lstAndLabel="i"
_lstOrLabel="OR"
_lstSelectedLabel="Wybrano"
_lstQuickFilterLab="Dodaj szybki filtr"

_openMenu="Kliknij tutaj, aby uzyskać dostęp do opcji {0}"
_openCalendarLab="Otwórz kalendarz"

_scroll_first_tab="Przewiń do pierwszej karty"
_scroll_previous_tab="Przewiń do poprzedniej karty"
_scroll_next_tab="Przewiń do następnej karty"
_scroll_last_tab="Przewiń do ostatniej karty"

_expandedLab="Rozwinięty"
_collapsedLab="Zwinięty"
_selectedLab="Wybrano"

_expandNode="Rozwiń węzeł %1"
_collapseNode="Zwiń węzeł %1"

_checkedPromptLab="Ustawiony"
_nocheckedPromptLab="Nieustawiony"
_selectionPromptLab="wartości równe"
_noselectionPromptLab="bez wartości"

_lovTextFieldLab="Tutaj wpisz wartości"
_lovCalendarLab="Tutaj wpisz datę"
_lovPrevChunkLab="Przejdź do poprzedniej porcji danych"
_lovNextChunkLab="Przejdź do następnej porcji danych"
_lovComboChunkLab="Porcja danych"
_lovRefreshLab="Odśwież"
_lovSearchFieldLab="Tutaj wpisz tekst do wyszukania"
_lovSearchLab="Wyszukaj"
_lovNormalLab="Normalna"
_lovMatchCase="Uwzględnij wielkość liter"
_lovRefreshValuesLab="Odśwież wartości"

_calendarNextMonthLab="Przejdź do następnego miesiąca"
_calendarPrevMonthLab="Przejdź do poprzedniego miesiąca"
_calendarNextYearLab="Przejdź do następnego roku"
_calendarPrevYearLab="Przejdź do poprzedniego roku"
_calendarSelectionLab="Wybrany dzień"

_menuCheckLab="Zaznaczone"
_menuDisableLab="Wyłączone"

_level="Poziom"
_closeTab="Zamknij kartę"
_of=" z "

_RGBTxtBegin= "RGB("
_RGBTxtEnd= ")"

_helpLab="Pomoc"

_waitTitleLab="Czekaj"
_cancelButtonLab="Anuluj"

_modifiers= new Array
_modifiers[0]="Ctrl+"
_modifiers[1]="Shift+"
_modifiers[2]="Alt+"

_bordersMoreColorsLabel="Więcej krawędzi..."
_bordersTooltip=new Array
_bordersTooltip[0]="Bez obramowania"
_bordersTooltip[1]="Lewa krawędź"
_bordersTooltip[2]="Prawa krawędź"
_bordersTooltip[3]="Krawędź dolna"
_bordersTooltip[4]="Średnia dolna krawędź"
_bordersTooltip[5]="Gruba dolna krawędź"
_bordersTooltip[6]="Górna i dolna krawędź"
_bordersTooltip[7]="Górna i średnia dolna krawędź"
_bordersTooltip[8]="Górna i gruba dolna krawędź"
_bordersTooltip[9]="Wszystkie krawędzie"
_bordersTooltip[10]="Wszystkie średnie krawędzie"
_bordersTooltip[11]="Wszystkie grube krawędzie"/* Copyright (c) Business Objects 2006. All rights reserved. */

// LOCALIZATION STRING

// Strings for calendar.js and calendar_param.js
var L_Today     = "Dzisiaj";
var L_January   = "Stycze\u0144";
var L_February  = "Luty";
var L_March     = "Marzec";
var L_April     = "Kwiecie\u0144";
var L_May       = "Maj";
var L_June      = "Czerwiec";
var L_July      = "Lipiec";
var L_August    = "Sierpie\u0144";
var L_September = "Wrzesie\u0144";
var L_October   = "Pa\u017Adziernik";
var L_November  = "Listopad";
var L_December  = "Grudzie\u0144";
var L_Su        = "Ni";
var L_Mo        = "Pn";
var L_Tu        = "Wt";
var L_We        = "\u015Ar";
var L_Th        = "Cz";
var L_Fr        = "Pt";
var L_Sa        = "So";

// Strings for prompts.js and prompts_param.js
var L_YYYY          = "rrrr";
var L_MM            = "mm";
var L_DD            = "dd";
var L_BadNumber     = "Ten parametr jest typu \"Liczba\" i mo\u017Ce zawiera\u0107 wy\u0142\u0105cznie symbol znaku minus, cyfry (\"0-9\"), znaki separator\u00F3w grupowania cyfr oraz znak separatora dziesi\u0119tnego. Popraw wprowadzon\u0105 warto\u015B\u0107.";
var L_BadCurrency   = "Ten parametr jest typu \"Waluta\" i mo\u017Ce zawiera\u0107 wy\u0142\u0105cznie symbol znaku minus, cyfry (\"0-9\"), znaki separator\u00F3w grupowania cyfr oraz znak separatora dziesi\u0119tnego. Popraw wprowadzon\u0105 warto\u015B\u0107.";
var L_BadDate       = "Ten parametr jest typu \"Data\" i powinien mie\u0107 format \"%1\", gdzie \"rrrr\" to rok w formacie czterocyfrowym, \"mm\" \u2014 miesi\u0105c (np. stycze\u0144 = 1), a \"dd\" \u2014 dzie\u0144 miesi\u0105ca.";
var L_BadDateTime   = "Ten parametr jest typu \"Data/godzina\" i jego poprawny format to \"%1 gg:mm:ss\", gdzie \"rrrr\" to rok w formacie czterocyfrowym, \"mm\" \u2014 miesi\u0105c (np. stycze\u0144 = 1), \"dd\" \u2014 dzie\u0144 miesi\u0105ca, \"gg\" \u2014 godziny wg zegara 24-godzinnego, \"mm\" \u2014 minuty, a \"ss\" \u2014 sekundy.";
var L_BadTime       = "Ten parametr jest typu \"Godzina\" i powinien mie\u0107 format \"gg:mm:ss\", gdzie \"gg\" to godziny wg zegara 24-godzinnego, \"mm\" \u2014 minuty, a \"ss\" \u2014 sekundy.";
var L_NoValue       = "Brak warto\u015Bci";
var L_BadValue      = "Aby ustawi\u0107 warto\u015B\u0107 \"Brak warto\u015Bci\", trzeba j\u0105 wybra\u0107 zar\u00F3wno dla warto\u015Bci Od, jak i Do.";
var L_BadBound      = "Nie mo\u017Cna jednocze\u015Bnie ustawi\u0107 warto\u015Bci \"Brak dolnej granicy\" i \"Brak g\u00F3rnej granicy\".";
var L_NoValueAlready = "Ten parametr ma ju\u017C ustawion\u0105 warto\u015B\u0107 \"Brak warto\u015Bci\". Usu\u0144 warto\u015B\u0107 \"Brak warto\u015Bci\" przed dodaniem innych warto\u015Bci.";
var L_RangeError    = "Warto\u015B\u0107 pocz\u0105tku zakresu nie mo\u017Ce by\u0107 wi\u0119ksza ni\u017C warto\u015B\u0107 ko\u0144ca zakresu.";
var L_NoDateEntered = "Musisz wprowadzi\u0107 dat\u0119.";
var L_Empty         = "Wprowad\u017A warto\u015B\u0107.";

// Strings for filter dialog
var L_closeDialog="Zamknij okno";

var L_SetFilter = "Ustaw filtr";
var L_OK        = "OK ";
var L_Cancel    = "Anuluj";

 /* Crystal Decisions Confidential Proprietary Information */