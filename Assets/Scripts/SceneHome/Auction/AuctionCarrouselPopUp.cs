using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionCarrouselPopUp : BasePage {
	public ScrollSnapRect scrollSnap;
	public string[] imageUrl;
	public ImageLoader[] imageContent;

	void Start()
	{
		scrollSnap.OnChangePage += OnChangePage;
	}

	protected override void Init()
	{
		scrollSnap.UpdateContainerSize (0);		
		imageContent [0].LoadImageFromUrl (imageUrl[0]);
	}

	public void OnChangePage(Transform child)
	{
		int index = child.GetSiblingIndex ();
		imageContent [index].LoadImageFromUrl (imageUrl[index]);
	}

    public void ClickClose()
    {
        SoundManager.Instance.PlaySFX(SFXList.Button02);
        Activate(false);
    }

	void OnDestroy()
	{
		scrollSnap.OnChangePage -= OnChangePage;
	}

}
