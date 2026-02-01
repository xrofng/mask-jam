using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitileCredit : BetterMonoBehaviour
{
    [SerializeField] Transform BeginHole;
    [SerializeField] PlayerInteractor Interactor;
    [SerializeField] MMF_Player BeginFB;
    [SerializeField] SimpleMMSoundPlayer IntroSfx;

    [SerializeField] Image Image;
    [SerializeField] List<Sprite> SlideShows = new List<Sprite>();

    private float _distance;
    private float _currDistance;
    private float _prevDis;

    protected override void Start()
    {
        base.Start();
        BeginFB?.PlayFeedbacks();
        _distance = Vector3.Distance(Interactor.transform.position, BeginHole.transform.position);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _currDistance = Vector3.Distance(Interactor.transform.position, BeginHole.transform.position);

        float speed = Mathf.Lerp(1, 3, 1 - (_currDistance - _distance));
        BeginFB.TimescaleMultiplier = speed;

        if (_currDistance < 3 && _prevDis >= 3)
        {
            StartCoroutine(GestaltLogoRoutine(4));
        }

        _prevDis = _currDistance;
    }

    IEnumerator GestaltLogoRoutine(float duration)
    {
        IntroSfx?.PlayClip();
        yield return new WaitForSeconds(1f);
        duration -= 1f;
        Image.enabled  = true;
        foreach (var sprite in SlideShows)
        {
            Image.sprite = sprite;
            yield return new WaitForSeconds(duration / (float)SlideShows.Count);
        }
        yield return new WaitForSeconds(2);
        Image.enabled  = false;
    }
}
