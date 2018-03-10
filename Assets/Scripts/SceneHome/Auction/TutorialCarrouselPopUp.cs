using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCarrouselPopUp : BasePage {
	public ScrollSnapRect scrollSnap;

	protected override void Init()
	{
		scrollSnap.UpdateContainerSize (0);		
	}

    public void ClickClose()
    {
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        Activate(false);
    }

}
