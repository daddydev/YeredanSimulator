﻿using UnityEngine;

// Token: 0x0200004A RID: 74
public class BoundaryScript : MonoBehaviour {

  // Token: 0x06000108 RID: 264 RVA: 0x00011D90 File Offset: 0x00010190
  private void Update() {
    float z = this.Yandere.position.z;
    if (z < -94f) {
      this.Intensity = -95f + Mathf.Abs(z);
      this.TextureCycle.Sprite.enabled = true;
      this.TextureCycle.enabled = true;
      Color color = this.Static.color + new Color(0.0001f, 0.0001f, 0.0001f, 0.0001f);
      Color color2 = this.Label.color;
      color.a = this.Intensity / 5f;
      color2.a = this.Intensity / 5f;
      this.Static.color = color;
      this.Label.color = color2;
      base.GetComponent<AudioSource>().volume = this.Intensity / 5f * 0.1f;
      Vector3 localPosition = this.Label.transform.localPosition;
      localPosition.x = UnityEngine.Random.Range(-10f, 10f);
      localPosition.y = UnityEngine.Random.Range(-10f, 10f);
      this.Label.transform.localPosition = localPosition;
    } else if (this.TextureCycle.enabled) {
      this.TextureCycle.Sprite.enabled = false;
      this.TextureCycle.enabled = false;
      Color color3 = this.Static.color;
      Color color4 = this.Label.color;
      color3.a = 0f;
      color4.a = 0f;
      this.Static.color = color3;
      this.Label.color = color4;
      base.GetComponent<AudioSource>().volume = 0f;
    }
  }

  // Token: 0x04000382 RID: 898
  public TextureCycleScript TextureCycle;

  // Token: 0x04000383 RID: 899
  public Transform Yandere;

  // Token: 0x04000384 RID: 900
  public UITexture Static;

  // Token: 0x04000385 RID: 901
  public UILabel Label;

  // Token: 0x04000386 RID: 902
  public float Intensity;
}