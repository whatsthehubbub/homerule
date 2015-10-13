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

	// TODO put the museum kids abstraction stuff in here
	
	public Goal GetStartGoal() {
		Goal g = new Goal();
		g.goalText = this.startGoalText;
		g.overlayText = this.startGoalOverlayText;
		g.locationSprite = this.startGoalLocationSprite;
		
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
	}


}