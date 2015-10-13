using System;
using System.Collections;


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

	// TODO put the delays in here

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
	}
}

public class AirborneMuseum : Museum {
	public AirborneMuseum() {
		this.startGoalText = "Zoek het geweer";
		this.startGoalOverlayText = "Ga op zoek naar het geweer. Het staat op de eerste verdieping.";
		this.startGoalLocationSprite = "geweer";

		this.location1GoalText = "Zoek het behang";
		this.location1GoalOverlayText = "Ga op zoek naar het behang. Dat hangt op de eerste verdieping.";
		this.location1LocationSprite = "behang";

		this.idleGoalText = "Verken het museum";
		this.idleGoalOverlayText = "Voel je vrij om het museum te verkennen. Je wordt gebeld als iemand je nodig heeft.";
		this.idleGoalLocationSprite = "";

		this.location2GoalText = "Zoek het bord";
		this.location2GoalOverlayText = "Ga op zoek naar het bord “Verboden Arnhem te betreden”. Dit hangt op de begane grond.";
		this.location2LocationSprite = "bord";

		this.location3GoalText = "Zoek de foto";
		this.location3GoalOverlayText = "Ga op zoek naar de foto van koningin Wilhelmina. Deze hangt bij de trap tussen de eerste verdieping en de begane grond.";
		this.location3LocationSprite = "wilhelmina";

		this.backToStartGoalText = "Ga terug naar het geweer";
		this.backToStartGoalOverlayText = "Ga terug naar het geweer op de eerste verdieping.";
		this.backToStartGoalLocationSprite = "geweer";
	}


}