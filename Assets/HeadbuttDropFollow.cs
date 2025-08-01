using UnityEngine;

public class HeadbuttDropFollow : MonoBehaviour
{
    Vector3 _velocity = Vector3.zero;
    private float smoothTime = .2f;
    public void StartFollow()
    {
        this.gameObject.SetActive(true);
        smoothTime = Random.Range(.1f, .3f);
    }

    void Update()
    {
        Vector3 screenPos = UIManager.Instance.hbPointsHUD.transform.position;
        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 40f));

        float _distance = Vector3.Distance(transform.position, worldTarget);
        if (_distance > .5f)
        {
            this.gameObject.transform.position = Vector3.SmoothDamp(transform.position, worldTarget, ref _velocity, smoothTime * Random.Range(1f, 4));
        }
        else
        {
            EndFollow();
        }


    }

    public void EndFollow()
    {
        UIManager.Instance.hbPointsHUD.AnimateBounce();
        this.gameObject.SetActive(false);
    }
}
