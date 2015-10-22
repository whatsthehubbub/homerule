using System;
using System.Collections;
using System.Collections.Generic;


public struct Goal {
	public string goalText;
	public string overlayText;
	public string locationSprite;

	public string GetGoalText() {
		return this.goalText;
	}

	public string GetOverlayText() {
		return this.overlayText;
	}
}


public abstract class Museum {
	public int major;

	public Dictionary<string, float> callDelays;

	public string museumCode;

	/* Introduction Screens */

	public string museumIntroScope;

	public string museumOverlayScope;

	/* Goal texts and images. */
	public string startGoalText;
	public string startGoalOverlayText;
	public string startGoalLocationSprite;

	public string location1GoalText;
	public string location1GoalOverlayText;
	public string location1LocationSprite;

	public string location2GoalText;
	public string location2GoalOverlayText;
	public string location2LocationSprite;

	public string location3GoalText;
	public string location3GoalOverlayText;
	public string location3LocationSprite;

	public string idleGoalText;
	public string idleGoalOverlayText;
	public string idleGoalLocationSprite;

	public string backToStartGoalText;
	public string backToStartGoalOverlayText;
	public string backToStartGoalLocationSprite;

	/* Story routing */

	public string museumName; // ReporterStory0

	public string goToFirstLocation;
	public string confirmGoToFirstLocation;

	public string goToSecondLocation;

	public string goToThirdLocation;
	public string arrivedAtThirdLocation;

	public string backToStartAdversarial;
	public string backToStartFriendly;

	/* Story questions and answers. */
	public string story1QuestionPre;
	public string[] story1QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story1QuestionAnswerResponse;
	public string story1QuestionOutro;
	public string story1ArticleIntro;

	public string story2QuestionPre;
	public string[] story2QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story2QuestionAnswerResponse;
	public string story2QuestionOutro;
	public string story2ArticleIntro;

	public string story3QuestionPre;
	public string[] story3QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story3QuestionAnswerResponse;

	public string story3QuestionWhy;
	public string[] story3QuestionWhyAnswer;

	public string story3QuestionOutro;
	public string story3ArticleIntro;


	// TODO put the museum kids abstraction stuff in here

	public Goal GetStartGoal() {
		Goal g = default(Goal);
		g.goalText = this.startGoalText;
		g.overlayText = this.startGoalOverlayText;
		g.locationSprite = this.startGoalLocationSprite;

		return g;
	}

	public Goal GetLocation1Goal() {
		Goal g = default(Goal);
		g.goalText = this.location1GoalText;
		g.overlayText = this.location1GoalOverlayText;
		g.locationSprite = this.location1LocationSprite;

		return g;
	}

	public Goal GetLocation2Goal() {
		Goal g = default(Goal);
		g.goalText = this.location2GoalText;
		g.overlayText = this.location2GoalOverlayText;
		g.locationSprite = this.location2LocationSprite;

		return g;
	}

	public Goal GetLocation3Goal() {
		Goal g = default(Goal);
		g.goalText = this.location3GoalText;
		g.overlayText = this.location3GoalOverlayText;
		g.locationSprite = this.location3LocationSprite;

		return g;
	}

	public Goal GetIdleGoal() {
		Goal g = default(Goal);
		g.goalText = this.idleGoalText;
		g.overlayText = this.idleGoalOverlayText;
		g.locationSprite = this.idleGoalLocationSprite;

		return g;
	}

	public Goal GetBackToStartGoal() {
		Goal g = default(Goal);
		g.goalText = this.backToStartGoalText;
		g.overlayText = this.backToStartGoalOverlayText;
		g.locationSprite = this.backToStartGoalLocationSprite;

		return g;
	}
}

public class DummyMuseum : Museum {
	public DummyMuseum() {
		this.major = 51895;

		this.callDelays = new Dictionary<string, float> {
			{"OFFICERRESPONSE1", 10.0f},
			{"REPORTERRESPONSE1", 10.0f},
			{"OFFICERRESPONSE2", 10.0f},
			{"ARTISTRESPONSE1", 10.0f},
			{"REPORTERRESPONSE2", 10.0f},
			{"OFFICERRESPONSE3", 10.0f}
		};

		this.museumCode = "DUMMY";

		this.museumIntroScope = "DE TABLET WEET WAAR JE BENT IN HET MUSEUM. JE HOEFT ALLEEN TE ZOEKEN OP BEPAALDE PLAATSEN VAN HET <B>DUMMY MUSEUM</B>.";

		this.museumOverlayScope = "DIT SPEL ZICH AF IN EEN DEEL VAN HET <B>DUMMY MUSEUM</B>. BLIJF DAAR.";

		this.startGoalText = "ZOEK HET DUMMY BEGIN";
		this.startGoalOverlayText = "GA OP ZOEK NAAR HET DUMMY BEGIN. HET STAAT OP DE DUMMY VERDIEPING.";
		this.startGoalLocationSprite = "dummy/placeholder-foto-locatie-0";

		this.location1GoalText = "ZOEK HET OBJECT 1";
		this.location1GoalOverlayText = "GA OP ZOEK NAAR OBJECT 1. DAT HANGT OP DE EERSTE VERDIEPING.";
		this.location1LocationSprite = "dummy/placeholder-foto-locatie-1";

		this.idleGoalText = "VERKEN HET MUSEUM";
		this.idleGoalOverlayText = "VOEL JE VRIJ OM HET MUSEUM TE VERKENNEN. JE WORDT GEBELD ALS IEMAND JE NODIG HEEFT.";
		this.idleGoalLocationSprite = "";

		this.location2GoalText = "ZOEK HET OBJECT 2";
		this.location2GoalOverlayText = "GA OP ZOEK NAAR OBJECT 2. DIT HANGT ERGENS.";
		this.location2LocationSprite = "dummy/placeholder-foto-locatie-2";

		this.location3GoalText = "ZOEK OBJECT 3";
		this.location3GoalOverlayText = "GA OP ZOEK NAAR OBJECT 3. DEZE HANGT BIJ DE TRAP IN DUMMY.";
		this.location3LocationSprite = "dummy/placeholder-foto-locatie-3";

		this.backToStartGoalText = "GA TERUG NAAR HET DUMMY";
		this.backToStartGoalOverlayText = "GA TERUG NAAR DUMMY OP DE VERDIEPING.";
		this.backToStartGoalLocationSprite = "dummy/placeholder-foto-locatie-0";


		this.museumName = "HET DUMMY MUSEUM";

		this.goToFirstLocation = "FIJN. GA JE NAAR DEZE GANG? DAAR HANGT OBJECT 1.";
		this.confirmGoToFirstLocation = "OKÉ, IK GA OBJECT 1 ZOEKEN.";

		this.goToSecondLocation = "ZEG, KUN JE IN HET MUSEUM ZOEKEN NAAR OBJECT 2?";

		this.goToThirdLocation = "IK HEB EEN IDEE. KUN JE OP ZOEK GAAN NAAR OBJECT 3? IK LEG HET STRAKS WEL UIT.";
		this.arrivedAtThirdLocation = "BEN JE BIJ OBJECT 3?";

		this.backToStartAdversarial = "U MOET TERUG NAAR HET BEGIN.";
		this.backToStartFriendly = "GAAT U TERUG NAAR HET BEGIN ALS U KLAAR BENT IN HET MUSEUM?";


		/* Story 1 */
		this.story1QuestionPre = "IK WEET IETS. MAAK JIJ EEN FOTO VAN OBJECT 1?";

		this.story1QuestionIntro = new string[] {
			"INTRO BIJ OBJECT 1",
			"INTRO BIJ OBJECT 1",
			"VRAAG BIJ OBJECT 1"};

		this.story1QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story1QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>("AW 1",
		                                                                                "ANTWOORD 1",
		                                                                                new string[] {
			"FEEDBACK BIJ ANTWOORD 1",
			"FEEDBACK BIJ ANTWOORD 1",
			"FEEDBACK BIJ ANTWOORD 1",
			"FEEDBACK BIJ ANTWOORD 1"
		});
		this.story1QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>("AW 2",
		                                                                                "ANTWOORD 2",
		                                                                                new string[] {
			"FEEDBACK BIJ ANTWOORD 2"
		});
		this.story1QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>("AW 3",
		                                                                                "ANTWOORD 3",
		                                                                                new string[] {
			"FEEDBACK BIJ ANTWOORD 3",
			"FEEDBACK BIJ ANTWOORD 3"
		});

		this.story1QuestionOutro = "UITLEIDING BIJ OBJECT 1";

		this.story1ArticleIntro = "DUMMY ARTIKEL INLEIDING";

		/* Story 2 */
		this.story2QuestionPre = "IK WEET IETS. MAAK JIJ EEN FOTO VAN OBJECT 2?";

		this.story2QuestionIntro = new string[] {
			"INTRO BIJ OBJECT 2",
			"INTRO BIJ OBJECT 2",
			"VRAAG BIJ OBJECT 2"
		};

		this.story2QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story2QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>(
			"AW 1",
			"ANTWOORD 1",
			new string[] {
			"FEEDBACK BIJ ANTWOORD 1"
			}
		);
		this.story2QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>(
			"AW 2",
			"ANTWOORD 2",
			new string[] {
				"FEEDBACK BIJ ANTWOORD 2"
			}
		);
		this.story2QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>(
			"AW 3",
			"ANTWOORD 3",
			new string[] {
				"FEEDBACK BIJ ANTWOORD 3",
				"FEEDBACK BIJ ANTWOORD 3"
			}
		);

		this.story2QuestionOutro = "DUMMY OUTRO";

		this.story2ArticleIntro = "DUMMY ARTIKEL INLEIDING";

		/* Story 3 */
		this.story3QuestionPre = "IK WEET IETS. MAAK JIJ EEN FOTO VAN OBJECT 3?";

		this.story3QuestionIntro = new string[] {
			"INTRO BIJ OBJECT 3",
			"INTRO BIJ OBJECT 3",
			"VRAAG BIJ OBJECT 3"
		};

		this.story3QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story3QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>(
			"AW 1",
			"ANTWOORD 1",
			new string[] {
				"FEEDBACK BIJ ANTWOORD 1",
				"FEEDBACK BIJ ANTWOORD 1"
			}
		);
		this.story3QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>(
			"AW 2",
			"ANTWOORD 2",
			new string[] {
				"FEEDBACK BIJ ANTWOORD 2",
				"FEEDBACK BIJ ANTWOORD 2"
			}
		);
		this.story3QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>(
			"AW 3",
			"ANTWOORD 3",
			new string[] {
				"FEEDBACK BIJ ANTWOORD 3"
			}
		);

		this.story3QuestionWhy = "OBJECT 3 WAAROM";

		this.story3QuestionWhyAnswer = new string[] {
			"OBJECT 3 REDEN",
			"OBJECT 3 REDEN"
		};

		this.story3QuestionOutro = "DUMMY OUTRO";

		this.story3ArticleIntro = "DUMMY INLEIDING VAN HET ARTIKEL";
	}
}

public class AirborneMuseum : Museum {
	public AirborneMuseum() {
		this.major = 47042;

		this.callDelays = new Dictionary<string, float> {
			{"OFFICERRESPONSE1", 120.0f},
			{"REPORTERRESPONSE1", 10.0f},
			{"OFFICERRESPONSE2", 30.0f},
			{"ARTISTRESPONSE1", 10.0f},
			{"REPORTERRESPONSE2", 30.0f},
			{"OFFICERRESPONSE3", 10.0f}
		};

		this.museumCode = "Airborne";

		this.museumIntroScope = "De tablet weet waar je bent in het museum. Je hoeft alleen te zoeken op de begane grond en de eerste verdieping van het <b>Airborne Museum</b>.";

		this.museumOverlayScope = "Dit spel speelt zich alleen af op de begane grond en de eerste verdieping van het <b>Airborne Museum</b>. Daarbuiten hoef je dus niet te zoeken.";

		this.startGoalText = "Zoek het geweer";
		this.startGoalOverlayText = "Ga op zoek naar het geweer. Het staat op de eerste verdieping.";
		this.startGoalLocationSprite = "airborne/geweer";

		this.location1GoalText = "Zoek het behang";
		this.location1GoalOverlayText = "Ga op zoek naar het behang. Dat hangt op de eerste verdieping.";
		this.location1LocationSprite = "airborne/behang";

		this.idleGoalText = "Verken het museum";
		this.idleGoalOverlayText = "Voel je vrij om het museum te verkennen. Je wordt gebeld als iemand je nodig heeft.";
		this.idleGoalLocationSprite = "";

		this.location2GoalText = "Zoek het bord";
		this.location2GoalOverlayText = "Ga op zoek naar het bord “Verboden Arnhem te betreden”. Dit hangt op de begane grond.";
		this.location2LocationSprite = "airborne/bord";

		this.location3GoalText = "Zoek de foto";
		this.location3GoalOverlayText = "Ga op zoek naar de foto van koningin Wilhelmina. Deze hangt bij de trap tussen de eerste verdieping en de begane grond.";
		this.location3LocationSprite = "airborne/wilhelmina";

		this.backToStartGoalText = "Ga terug naar het geweer";
		this.backToStartGoalOverlayText = "Ga terug naar het geweer op de eerste verdieping.";
		this.backToStartGoalLocationSprite = "airborne/geweer";

		/* Story 0 */
		this.museumName = "het Airborne Museum";

		this.goToFirstLocation = "Fijn. Ga je naar deze gang? Daar hangt een oud stuk behang.";
		this.confirmGoToFirstLocation = "Oké, ik ga het behang zoeken.";

		this.goToSecondLocation = "Zeg, kun je in het museum zoeken naar het bord “Verboden Arnhem te betreden”?";

		this.goToThirdLocation = "Ik heb een idee. Kun je op zoek gaan naar de foto van koningin Wilhelmina? Ik leg het straks wel uit.";
		this.arrivedAtThirdLocation = "Ben je bij die foto?";

		this.backToStartAdversarial = "U moet terug naar het geweer.";
		this.backToStartFriendly = "Gaat u terug naar het geweer als u klaar bent in het museum?";

		/* Story 1 */
		this.story1QuestionPre = "Ik weet iets. Maak jij een foto van het stuk behang in het museum?";

		this.story1QuestionIntro = new string[] {
			"Een Engelse soldaat schreef in de oorlog op dat behang.", 
			"Waarom deed hij dat, denk jij? Je mag overleggen!", 
			"Was dat (1) om te tellen hoeveel Duitsers hij had neergeschoten, (2) omdat hij bang was, of (3) omdat hij graag muren bekladde?"};

		this.story1QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story1QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>("Tellen", 
		                                                                                "Ik denk om te tellen hoeveel Duitsers hij had neergeschoten.", 
		                                                                                new string[] {"Klopt! De soldaat telde hoeveel Duitsers hij neerschoot.",
			"Bedenk wel: hij zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee ook moed in."});
		this.story1QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>("Bang", 
		                                                                                "Ik denk omdat hij bang was.", 
		                                                                                new string[] {"Klopt! De soldaat zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee moed in."});
		this.story1QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>("Kladden", 
		                                                                                "Ik denk omdat hij graag muren bekladde.", 
		                                                                                new string[] {"Dat was niet de reden. De soldaat schaamt zich er nu zelfs voor.", 
			"Hij zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee moed in."});

		this.story1QuestionOutro = "Lelijke woorden hè? Maar voor de soldaat was het een belangrijke tekst.";
		this.story1ArticleIntro = "Als iemand op een muur schrijft, in de oorlog en nu, is dat niet altijd slecht bedoeld.";

		/* Story 2 */

		this.story2QuestionPre = "Ik weet iets. Maak jij een foto van het bord met “verboden Arnhem te betreden”?";

		this.story2QuestionIntro = new string[] {
			"De inwoners van Arnhem moesten ook hun huis uit.",
			"Waarom denk jij dat de Duitse bezetter dat wilde? Als je samen speelt, kun je overleggen.",
			"Was dat (1) omdat het er gevaarlijk was, (2) zodat inwoners de geallieerden niet konden helpen, of (3) zodat de Duitsers hun spullen konden stelen?"
		};

		this.story2QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story2QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>(
			"Gevaarlijk",
			"Ik denk omdat het er gevaarlijk was. Er werd gevochten en gebombardeerd.",
			new string[] {"Klopt! Maar de Duitse bezetter was ook bang dat inwoners de geallieerden zouden helpen."}
		);
		this.story2QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>(
			"Geallieerden helpen",
			"Ik denk zodat inwoners de geallieerden niet konden helpen.",
			new string[] {"Klopt! Daarnaast was het er gevaarlijk. Er werd gevochten en gebombardeerd."}
		);
		this.story2QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>(
			"Spullen stelen",
			"Ik denk zodat de Duitsers hun spullen konden stelen.",
			new string[] {"Dat was niet de reden, maar het gebeurde wel. Er werden spullen gestolen door Duitsers én door burgers in nood.",
						"Maar de mensen moesten weg omdat het er gevaarlijk was. Er werd gevochten en gebombardeerd."}
		);

		this.story2QuestionOutro = "Je moest binnen twee dagen weg. Daarna was Arnhem verboden terrein.";

		this.story2ArticleIntro = "De inwoners van Arnhem moesten hun huis uit vanwege gevaar, net als nu.";

		/* Story 3 */

		this.story3QuestionPre = "Kun je een foto maken van koningin Wilhelmina? Een foto van de foto?";

		this.story3QuestionIntro = new string[] {
			"In de oorlog mocht je niet laten zien dat je van het koningshuis hield.",
			"Waarom, denk jij? Je mag overleggen!",
			"Was dit omdat de Duitsers (1) tegen koningin Wilhelmina waren, (2) wilden dat je voor Hitler was, of (3) bang waren dat mensen zich zouden organiseren."
		};

		this.story3QuestionAnswerResponse = new Eppy.Tuple<string, string, string[]>[3];
		this.story3QuestionAnswerResponse[0] = new Eppy.Tuple<string, string, string[]>(
			"Tegen de koningin",
			"Ik denk omdat de Duitsers tegen koningin Wilhelmina waren.",
			new string[] {
				"Klopt! De Duitse bezetter was de baas. De koningin was gevlucht naar Engeland.",
				"Maar ze waren ook bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen."
			}
		);
		this.story3QuestionAnswerResponse[1] = new Eppy.Tuple<string, string, string[]>(
			"Voor Hitler",
			"Ik denk omdat ze wilden dat je voor Hitler was, hun leider.",
			new string[] {
				"Klopt! De Duitse bezetter was de baas.",
				"Maar ze waren ook bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen."
			}
		);
		this.story3QuestionAnswerResponse[2] = new Eppy.Tuple<string, string, string[]>(
			"Organiseren",
			"Ik denk dat ze bang waren dat mensen zich zouden organiseren.",
			new string[] {"Klopt! De Duitse bezetter was bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen."}
		);

		this.story3QuestionWhy = "Waarom waren de Duitsers daar bang voor?";

		this.story3QuestionWhyAnswer = new string[] {
			"Samen staan mensen sterk. Dat kon het Duitse gezag in gevaar brengen.",
			"Daarom verdween de vrijheid om je politieke ideeën te laten zien. Zelfs foto’s van de koningin werden verboden."
		};

		this.story3QuestionOutro = "De mensen konden niet meer zichzelf zijn. Net als de vogels nu. En de politie liegt erover!";

		this.story3ArticleIntro = "Vrije vogel of koningsgezind, je moet jezelf kunnen zijn!";
	}
}