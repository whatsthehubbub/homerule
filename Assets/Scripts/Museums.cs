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
	public string museumName;

	public int major;

	// TODO put the delays in here

	/* Introduction Screens */

	public string museumScope;

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

	/* Story questions and answers. */
	public string story1QuestionPre;
	public string[] story1QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story1QuestionAnswerResponse;
	public string story1QuestionOutro;

	public string story2QuestionPre;
	public string[] story2QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story2QuestionAnswerResponse;
	public string story2QuestionOutro;

	public string story3QuestionPre;
	public string[] story3QuestionIntro;
	public Eppy.Tuple<string, string, string[]>[] story3QuestionAnswerResponse;
	public string story3QuestionOutro;


	// TODO put the museum kids abstraction stuff in here
	
	public Goal GetStartGoal() {
		Goal g = default(Goal);

	public Goal GetStartGoal() {
		Goal g = new Goal();
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
		this.museumName = "het DUMMY MUSEUM";

		this.museumScope = "De tablet weet waar je bent in het museum. Je hoeft alleen te zoeken op bepaalde plaatsen van het <b>DUMMY MUSEUM</b>.";

		this.startGoalText = "Zoek het DUMMY begin";
		this.startGoalOverlayText = "Ga op zoek naar het DUMMY begin. Het staat op de DUMMY verdieping.";
		this.startGoalLocationSprite = "dummy/placeholder-foto-locatie-0";
	}
}

public class AirborneMuseum : Museum {
	public AirborneMuseum() {
		this.museumName = "het Airborne Museum";

		this.museumScope = "De tablet weet waar je bent in het museum. Je hoeft alleen te zoeken op de begane grond en de eerste verdieping van het <b>Airborne Museum</b>.";

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

	}


}