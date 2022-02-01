using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    public bool isRecord;
    public bool isReplay;

    public List<Vector3> recPosition;
    public List<Quaternion> recRotation;
    public List<bool> recFlipX;
    public List<Sprite> recSprite;

    private int recordOnceEvery = 1;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int frameCount = Time.frameCount;
        if (isRecord && frameCount % recordOnceEvery == 0)
        {
            recPosition.Add(this.transform.position);
            recRotation.Add(this.transform.rotation);
            recFlipX.Add(spriteRenderer.flipX);
            recSprite.Add(spriteRenderer.sprite);
        }
        else if (isReplay)
        {
            int ind = frameCount % recPosition.Count;
            this.transform.position = recPosition[ind];
            this.transform.rotation = recRotation[ind];
            spriteRenderer.flipX = recFlipX[ind];
            spriteRenderer.sprite = recSprite[ind];
        }
    }

    public void clear()
    {
        recPosition.Clear();
        recRotation.Clear();
        recFlipX.Clear();
        recSprite.Clear();
    }
}
