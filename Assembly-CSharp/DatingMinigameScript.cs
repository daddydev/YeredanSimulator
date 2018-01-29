﻿using UnityEngine;

// Token: 0x0200007D RID: 125
public class DatingMinigameScript : MonoBehaviour {

  // Token: 0x060001EB RID: 491 RVA: 0x00025EEC File Offset: 0x000242EC
  private void Start() {
    this.Affection = DatingGlobals.Affection;
    this.AffectionBar.localScale = new Vector3(this.Affection / 100f, this.AffectionBar.localScale.y, this.AffectionBar.localScale.z);
    this.CalculateAffection();
    this.OriginalColor = this.ComplimentBGs[1].color;
    this.ComplimentSet.localScale = Vector3.zero;
    this.GiveGift.localScale = Vector3.zero;
    this.ShowOff.localScale = Vector3.zero;
    this.Topics.localScale = Vector3.zero;
    this.DatingSimHUD.gameObject.SetActive(false);
    this.DatingSimHUD.alpha = 0f;
    for (int i = 1; i < 26; i++) {
      if (DatingGlobals.GetTopicDiscussed(i)) {
        UISprite uisprite = this.TopicIcons[i];
        uisprite.color = new Color(uisprite.color.r, uisprite.color.g, uisprite.color.b, 0.5f);
      }
    }
    for (int j = 1; j < 11; j++) {
      if (DatingGlobals.GetComplimentGiven(j)) {
        UILabel uilabel = this.ComplimentLabels[j];
        uilabel.color = new Color(uilabel.color.r, uilabel.color.g, uilabel.color.b, 0.5f);
      }
    }
    this.UpdateComplimentHighlight();
    this.UpdateTraitHighlight();
    this.UpdateGiftHighlight();
  }

  // Token: 0x060001EC RID: 492 RVA: 0x000260A8 File Offset: 0x000244A8
  private void CalculateAffection() {
    if (this.Affection == 0f) {
      this.AffectionLevel = 0;
    } else if (this.Affection < 25f) {
      this.AffectionLevel = 1;
    } else if (this.Affection < 50f) {
      this.AffectionLevel = 2;
    } else if (this.Affection < 75f) {
      this.AffectionLevel = 3;
    } else if (this.Affection < 100f) {
      this.AffectionLevel = 4;
    } else {
      this.AffectionLevel = 5;
    }
  }

  // Token: 0x060001ED RID: 493 RVA: 0x00026148 File Offset: 0x00024548
  private void Update() {
    if (this.Testing) {
      this.Prompt.enabled = true;
    } else if (this.LoveManager.RivalWaiting) {
      if (this.Rival == null) {
        this.Suitor = this.StudentManager.Students[13];
        this.Rival = this.StudentManager.Students[7];
      }
      if (this.Rival.MeetTimer > 0f && this.Suitor.MeetTimer > 0f) {
        this.Prompt.enabled = true;
      }
    } else if (this.Prompt.enabled) {
      this.Prompt.Hide();
      this.Prompt.enabled = false;
    }
    if (this.Prompt.Circle[0].fillAmount == 0f) {
      this.Suitor.enabled = false;
      this.Rival.enabled = false;
      this.Rival.Character.GetComponent<Animation>()["f02_smile_00"].layer = 1;
      this.Rival.Character.GetComponent<Animation>().Play("f02_smile_00");
      this.Rival.Character.GetComponent<Animation>()["f02_smile_00"].weight = 0f;
      this.StudentManager.Clock.StopTime = true;
      this.Yandere.RPGCamera.enabled = false;
      this.HeartbeatCamera.SetActive(false);
      this.Yandere.Headset.SetActive(true);
      this.Yandere.CanMove = false;
      this.Yandere.EmptyHands();
      this.Yandere.transform.position = this.PeekSpot.position;
      this.Yandere.transform.eulerAngles = this.PeekSpot.eulerAngles;
      this.Yandere.Character.GetComponent<Animation>().Play("f02_treePeeking_00");
      Camera.main.transform.position = new Vector3(48f, 3f, -44f);
      Camera.main.transform.eulerAngles = new Vector3(15f, 90f, 0f);
      this.WisdomLabel.text = "Wisdom: " + DatingGlobals.GetSuitorTrait(2).ToString();
      if (!this.Suitor.Rose) {
        this.RoseIcon.enabled = false;
      }
      this.Matchmaking = true;
      this.UpdateTopics();
    }
    if (this.Matchmaking) {
      if (this.CurrentAnim != string.Empty && this.Rival.Character.GetComponent<Animation>()[this.CurrentAnim].time >= this.Rival.Character.GetComponent<Animation>()[this.CurrentAnim].length) {
        this.Rival.Character.GetComponent<Animation>().Play(this.Rival.IdleAnim);
      }
      if (this.Phase == 1) {
        this.Panel.alpha = Mathf.MoveTowards(this.Panel.alpha, 0f, Time.deltaTime);
        this.Timer += Time.deltaTime;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(54f, 1.25f, -45.25f), this.Timer * 0.02f);
        Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, new Vector3(0f, 45f, 0f), this.Timer * 0.02f);
        if (this.Timer > 5f) {
          this.Suitor.Character.GetComponent<Animation>().Play("insertEarpiece_00");
          this.Suitor.Character.GetComponent<Animation>()["insertEarpiece_00"].time = 0f;
          this.Suitor.Character.GetComponent<Animation>().Play("insertEarpiece_00");
          this.Suitor.Earpiece.SetActive(true);
          Camera.main.transform.position = new Vector3(45.5f, 1.25f, -44.5f);
          Camera.main.transform.eulerAngles = new Vector3(0f, -45f, 0f);
          this.Rotation = -45f;
          this.Timer = 0f;
          this.Phase++;
        }
      } else if (this.Phase == 2) {
        this.Timer += Time.deltaTime;
        if (this.Timer > 4f) {
          this.Suitor.Earpiece.transform.parent = this.Suitor.Head;
          this.Suitor.Earpiece.transform.localPosition = new Vector3(0f, -1.12f, 1.14f);
          this.Suitor.Earpiece.transform.localEulerAngles = new Vector3(45f, -180f, 0f);
          Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(45.11f, 1.375f, -44f), (this.Timer - 4f) * 0.02f);
          this.Rotation = Mathf.Lerp(this.Rotation, 90f, (this.Timer - 4f) * 0.02f);
          Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, this.Rotation, Camera.main.transform.eulerAngles.z);
          if (this.Rotation > 89.9f) {
            this.Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time = 0f;
            this.Rival.Character.GetComponent<Animation>().CrossFade("f02_turnAround_00");
            this.AffectionBar.localScale = new Vector3(this.Affection / 100f, this.AffectionBar.localScale.y, this.AffectionBar.localScale.z);
            this.DialogueLabel.text = this.Greetings[this.AffectionLevel];
            this.CalculateMultiplier();
            this.DatingSimHUD.gameObject.SetActive(true);
            this.Timer = 0f;
            this.Phase++;
          }
        }
      } else if (this.Phase == 3) {
        this.DatingSimHUD.alpha = Mathf.MoveTowards(this.DatingSimHUD.alpha, 1f, Time.deltaTime);
        if (this.Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time >= this.Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].length) {
          this.Rival.transform.eulerAngles = new Vector3(this.Rival.transform.eulerAngles.x, -90f, this.Rival.transform.eulerAngles.z);
          this.Rival.Character.GetComponent<Animation>().Play("f02_turnAround_00");
          this.Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].time = 0f;
          this.Rival.Character.GetComponent<Animation>()["f02_turnAround_00"].speed = 0f;
          this.Rival.Character.GetComponent<Animation>().Play("f02_turnAround_00");
          this.Rival.Character.GetComponent<Animation>().CrossFade(this.Rival.IdleAnim);
          this.PromptBar.ClearButtons();
          this.PromptBar.Label[0].text = "Confirm";
          this.PromptBar.Label[1].text = "Back";
          this.PromptBar.Label[4].text = "Select";
          this.PromptBar.UpdateButtons();
          this.PromptBar.Show = true;
          this.Phase++;
        }
      } else if (this.Phase == 4) {
        if (this.AffectionGrow) {
          this.Affection = Mathf.MoveTowards(this.Affection, 100f, Time.deltaTime * 10f);
          this.CalculateAffection();
        }
        this.Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", this.Affection * 0.01f);
        this.Rival.CharacterAnimation["f02_smile_00"].weight = this.Affection * 0.01f;
        this.Highlight.localPosition = new Vector3(this.Highlight.localPosition.x, Mathf.Lerp(this.Highlight.localPosition.y, this.HighlightTarget, Time.deltaTime * 10f), this.Highlight.localPosition.z);
        for (int i = 1; i < this.Options.Length; i++) {
          Transform transform = this.Options[i];
          transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, (i != this.Selected) ? 800f : 750f, Time.deltaTime * 10f), transform.localPosition.y, transform.localPosition.z);
        }
        this.AffectionBar.localScale = new Vector3(Mathf.Lerp(this.AffectionBar.localScale.x, this.Affection / 100f, Time.deltaTime * 10f), this.AffectionBar.localScale.y, this.AffectionBar.localScale.z);
        if (!this.SelectingTopic && !this.Complimenting && !this.ShowingOff && !this.GivingGift) {
          this.Topics.localScale = Vector3.Lerp(this.Topics.localScale, Vector3.zero, Time.deltaTime * 10f);
          this.ComplimentSet.localScale = Vector3.Lerp(this.ComplimentSet.localScale, Vector3.zero, Time.deltaTime * 10f);
          this.ShowOff.localScale = Vector3.Lerp(this.ShowOff.localScale, Vector3.zero, Time.deltaTime * 10f);
          this.GiveGift.localScale = Vector3.Lerp(this.GiveGift.localScale, Vector3.zero, Time.deltaTime * 10f);
          if (this.InputManager.TappedUp) {
            this.Selected--;
            this.UpdateHighlight();
          }
          if (this.InputManager.TappedDown) {
            this.Selected++;
            this.UpdateHighlight();
          }
          if (Input.GetButtonDown("A") && this.Labels[this.Selected].color.a == 1f) {
            if (this.Selected == 1) {
              this.SelectingTopic = true;
              this.Negative = true;
            } else if (this.Selected == 2) {
              this.SelectingTopic = true;
              this.Negative = false;
            } else if (this.Selected == 3) {
              this.Complimenting = true;
            } else if (this.Selected == 4) {
              this.ShowingOff = true;
            } else if (this.Selected == 5) {
              this.GivingGift = true;
            } else if (this.Selected == 6) {
              this.PromptBar.ClearButtons();
              this.PromptBar.Label[0].text = "Confirm";
              this.PromptBar.UpdateButtons();
              this.CalculateAffection();
              this.DialogueLabel.text = this.Farewells[this.AffectionLevel];
              this.Phase++;
            }
          }
        } else if (this.SelectingTopic) {
          this.Topics.localScale = Vector3.Lerp(this.Topics.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
          if (this.InputManager.TappedUp) {
            this.Row--;
            this.UpdateTopicHighlight();
          } else if (this.InputManager.TappedDown) {
            this.Row++;
            this.UpdateTopicHighlight();
          }
          if (this.InputManager.TappedLeft) {
            this.Column--;
            this.UpdateTopicHighlight();
          } else if (this.InputManager.TappedRight) {
            this.Column++;
            this.UpdateTopicHighlight();
          }
          if (Input.GetButtonDown("A") && this.TopicIcons[this.TopicSelected].color.a == 1f) {
            this.SelectingTopic = false;
            UISprite uisprite = this.TopicIcons[this.TopicSelected];
            uisprite.color = new Color(uisprite.color.r, uisprite.color.g, uisprite.color.b, 0.5f);
            DatingGlobals.SetTopicDiscussed(this.TopicSelected, true);
            this.DetermineOpinion();
            if (!ConversationGlobals.GetTopicLearnedByStudent(this.Opinion, 7)) {
              ConversationGlobals.SetTopicLearnedByStudent(this.Opinion, 7, true);
            }
            if (this.Negative) {
              UILabel uilabel = this.Labels[1];
              uilabel.color = new Color(uilabel.color.r, uilabel.color.g, uilabel.color.b, 0.5f);
              if (this.Opinion == 2) {
                this.DialogueLabel.text = "Hey! Just so you know, I take offense to that...";
                this.Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
                this.CurrentAnim = "f02_refuse_00";
                this.Affection -= 1f;
                this.CalculateAffection();
              } else if (this.Opinion == 1) {
                this.DialogueLabel.text = this.Negatives[this.AffectionLevel];
                this.Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
                this.CurrentAnim = "f02_lookdown_00";
                this.Affection += (float)this.Multiplier;
                this.CalculateAffection();
              } else if (this.Opinion == 0) {
                this.DialogueLabel.text = "Um...okay.";
              }
            } else {
              UILabel uilabel2 = this.Labels[2];
              uilabel2.color = new Color(uilabel2.color.r, uilabel2.color.g, uilabel2.color.b, 0.5f);
              if (this.Opinion == 2) {
                this.DialogueLabel.text = this.Positives[this.AffectionLevel];
                this.Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
                this.CurrentAnim = "f02_lookdown_00";
                this.Affection += (float)this.Multiplier;
                this.CalculateAffection();
              } else if (this.Opinion == 1) {
                this.DialogueLabel.text = "To be honest with you, I strongly disagree...";
                this.Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
                this.CurrentAnim = "f02_refuse_00";
                this.Affection -= 1f;
                this.CalculateAffection();
              } else if (this.Opinion == 0) {
                this.DialogueLabel.text = "Um...all right.";
              }
            }
            if (this.Affection > 100f) {
              this.Affection = 100f;
            } else if (this.Affection < 0f) {
              this.Affection = 0f;
            }
          }
          if (Input.GetButtonDown("B")) {
            this.SelectingTopic = false;
          }
        } else if (this.Complimenting) {
          this.ComplimentSet.localScale = Vector3.Lerp(this.ComplimentSet.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
          if (this.InputManager.TappedUp) {
            this.Line--;
            this.UpdateComplimentHighlight();
          } else if (this.InputManager.TappedDown) {
            this.Line++;
            this.UpdateComplimentHighlight();
          }
          if (this.InputManager.TappedLeft) {
            this.Side--;
            this.UpdateComplimentHighlight();
          } else if (this.InputManager.TappedRight) {
            this.Side++;
            this.UpdateComplimentHighlight();
          }
          if (Input.GetButtonDown("A") && this.ComplimentLabels[this.ComplimentSelected].color.a == 1f) {
            UILabel uilabel3 = this.Labels[3];
            uilabel3.color = new Color(uilabel3.color.r, uilabel3.color.g, uilabel3.color.b, 0.5f);
            this.Complimenting = false;
            this.DialogueLabel.text = this.Compliments[this.ComplimentSelected];
            DatingGlobals.SetComplimentGiven(this.ComplimentSelected, true);
            if (this.ComplimentSelected == 1 || this.ComplimentSelected == 4 || this.ComplimentSelected == 5 || this.ComplimentSelected == 8 || this.ComplimentSelected == 9) {
              this.Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
              this.CurrentAnim = "f02_lookdown_00";
              this.Affection += (float)this.Multiplier;
              this.CalculateAffection();
            } else {
              this.Rival.Character.GetComponent<Animation>().CrossFade("f02_refuse_00");
              this.CurrentAnim = "f02_refuse_00";
              this.Affection -= 1f;
              this.CalculateAffection();
            }
            if (this.Affection > 100f) {
              this.Affection = 100f;
            } else if (this.Affection < 0f) {
              this.Affection = 0f;
            }
          }
          if (Input.GetButtonDown("B")) {
            this.Complimenting = false;
          }
        } else if (this.ShowingOff) {
          this.ShowOff.localScale = Vector3.Lerp(this.ShowOff.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
          if (this.InputManager.TappedUp) {
            this.TraitSelected--;
            this.UpdateTraitHighlight();
          } else if (this.InputManager.TappedDown) {
            this.TraitSelected++;
            this.UpdateTraitHighlight();
          }
          if (Input.GetButtonDown("A")) {
            UILabel uilabel4 = this.Labels[4];
            uilabel4.color = new Color(uilabel4.color.r, uilabel4.color.g, uilabel4.color.b, 0.5f);
            this.ShowingOff = false;
            if (this.TraitSelected == 2) {
              if (DatingGlobals.GetSuitorTrait(2) > DatingGlobals.GetTraitDemonstrated(2)) {
                DatingGlobals.SetTraitDemonstrated(2, DatingGlobals.GetTraitDemonstrated(2) + 1);
                this.DialogueLabel.text = this.ShowOffs[this.AffectionLevel];
                this.Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
                this.CurrentAnim = "f02_lookdown_00";
                this.Affection += (float)this.Multiplier;
                this.CalculateAffection();
              } else {
                this.DialogueLabel.text = "Uh...you already told me about that...";
              }
            } else {
              this.DialogueLabel.text = "Um...well...that sort of thing doesn't really matter to me...";
            }
            if (this.Affection > 100f) {
              this.Affection = 100f;
            } else if (this.Affection < 0f) {
              this.Affection = 0f;
            }
          }
          if (Input.GetButtonDown("B")) {
            this.ShowingOff = false;
          }
        } else if (this.GivingGift) {
          this.GiveGift.localScale = Vector3.Lerp(this.GiveGift.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
          if (this.InputManager.TappedUp) {
            this.GiftRow--;
            this.UpdateGiftHighlight();
          } else if (this.InputManager.TappedDown) {
            this.GiftRow++;
            this.UpdateGiftHighlight();
          }
          if (this.InputManager.TappedLeft) {
            this.GiftColumn--;
            this.UpdateGiftHighlight();
          } else if (this.InputManager.TappedRight) {
            this.GiftColumn++;
            this.UpdateGiftHighlight();
          }
          if (Input.GetButtonDown("A")) {
            if (this.GiftSelected == 1 && this.RoseIcon.enabled) {
              UILabel uilabel5 = this.Labels[5];
              uilabel5.color = new Color(uilabel5.color.r, uilabel5.color.g, uilabel5.color.b, 0.5f);
              this.GivingGift = false;
              this.DialogueLabel.text = this.GiveGifts[this.AffectionLevel];
              this.Rival.Character.GetComponent<Animation>().CrossFade("f02_lookdown_00");
              this.CurrentAnim = "f02_lookdown_00";
              this.Affection += (float)this.Multiplier;
              this.CalculateAffection();
            }
            if (this.Affection > 100f) {
              this.Affection = 100f;
            } else if (this.Affection < 0f) {
              this.Affection = 0f;
            }
          }
          if (Input.GetButtonDown("B")) {
            this.GivingGift = false;
          }
        }
      } else if (this.Phase == 5) {
        this.Speed += Time.deltaTime * 100f;
        this.AffectionSet.localPosition = new Vector3(this.AffectionSet.localPosition.x, this.AffectionSet.localPosition.y + this.Speed, this.AffectionSet.localPosition.z);
        this.OptionSet.localPosition = new Vector3(this.OptionSet.localPosition.x + this.Speed, this.OptionSet.localPosition.y, this.OptionSet.localPosition.z);
        if (this.Speed > 100f && Input.GetButtonDown("A")) {
          this.Phase++;
        }
      } else if (this.Phase == 6) {
        this.DatingSimHUD.alpha = Mathf.MoveTowards(this.DatingSimHUD.alpha, 0f, Time.deltaTime);
        if (this.DatingSimHUD.alpha == 0f) {
          this.DatingSimHUD.gameObject.SetActive(false);
          this.Phase++;
        }
      } else if (this.Phase == 7) {
        if (this.Panel.alpha == 0f) {
          this.LoveManager.RivalWaiting = false;
          this.LoveManager.Courted = true;
          this.Suitor.enabled = true;
          this.Rival.enabled = true;
          this.Suitor.CurrentDestination = this.Suitor.Destinations[this.Suitor.Phase];
          this.Suitor.Pathfinding.target = this.Suitor.Destinations[this.Suitor.Phase];
          this.Suitor.Prompt.Label[0].text = "     Talk";
          this.Suitor.Pathfinding.canSearch = true;
          this.Suitor.Pathfinding.canMove = true;
          this.Suitor.Pushable = false;
          this.Suitor.Meeting = false;
          this.Suitor.Routine = true;
          this.Suitor.MeetTimer = 0f;
          this.Rival.Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 0f);
          this.Rival.CurrentDestination = this.Rival.Destinations[this.Rival.Phase];
          this.Rival.Pathfinding.target = this.Rival.Destinations[this.Rival.Phase];
          this.Rival.CharacterAnimation["f02_smile_00"].weight = 0f;
          this.Rival.Prompt.Label[0].text = "     Talk";
          this.Rival.Pathfinding.canSearch = true;
          this.Rival.Pathfinding.canMove = true;
          this.Rival.Pushable = false;
          this.Rival.Meeting = false;
          this.Rival.Routine = true;
          this.Rival.MeetTimer = 0f;
          this.StudentManager.Clock.StopTime = false;
          this.Yandere.RPGCamera.enabled = true;
          this.Suitor.Earpiece.SetActive(false);
          this.HeartbeatCamera.SetActive(true);
          this.Yandere.Headset.SetActive(false);
          DatingGlobals.Affection = this.Affection;
          this.PromptBar.ClearButtons();
          this.PromptBar.Show = false;
        } else if (this.Panel.alpha == 1f) {
          this.Matchmaking = false;
          this.Yandere.CanMove = true;
          base.gameObject.SetActive(false);
        }
        this.Panel.alpha = Mathf.MoveTowards(this.Panel.alpha, 1f, Time.deltaTime);
      }
    }
  }

  // Token: 0x060001EE RID: 494 RVA: 0x00027D99 File Offset: 0x00026199
  private void LateUpdate() {
    if (this.Phase == 4) {
    }
  }

  // Token: 0x060001EF RID: 495 RVA: 0x00027DA8 File Offset: 0x000261A8
  private void CalculateMultiplier() {
    this.Multiplier = 5;
    if (!this.Suitor.Cosmetic.Eyewear[6].activeInHierarchy) {
      this.MultiplierIcons[1].mainTexture = this.X;
      this.Multiplier--;
    }
    if (!this.Suitor.Cosmetic.MaleAccessories[3].activeInHierarchy) {
      this.MultiplierIcons[2].mainTexture = this.X;
      this.Multiplier--;
    }
    if (!this.Suitor.Cosmetic.MaleHair[22].activeInHierarchy) {
      this.MultiplierIcons[3].mainTexture = this.X;
      this.Multiplier--;
    }
    if (this.Suitor.Cosmetic.HairColor != "Purple") {
      this.MultiplierIcons[4].mainTexture = this.X;
      this.Multiplier--;
    }
    if (PlayerGlobals.PantiesEquipped == 2) {
      this.PantyIcon.SetActive(true);
      this.Multiplier++;
    } else {
      this.PantyIcon.SetActive(false);
    }
    if (PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 0) {
      this.SeductionLabel.text = (PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus).ToString();
      this.Multiplier += PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus;
      this.SeductionIcon.SetActive(true);
    } else {
      this.SeductionIcon.SetActive(false);
    }
    this.MultiplierLabel.text = "Multiplier: " + this.Multiplier.ToString() + "x";
  }

  // Token: 0x060001F0 RID: 496 RVA: 0x00027F80 File Offset: 0x00026380
  private void UpdateHighlight() {
    if (this.Selected < 1) {
      this.Selected = 6;
    } else if (this.Selected > 6) {
      this.Selected = 1;
    }
    this.HighlightTarget = 450f - 100f * (float)this.Selected;
  }

  // Token: 0x060001F1 RID: 497 RVA: 0x00027FD4 File Offset: 0x000263D4
  private void UpdateTopicHighlight() {
    if (this.Row < 1) {
      this.Row = 5;
    } else if (this.Row > 5) {
      this.Row = 1;
    }
    if (this.Column < 1) {
      this.Column = 5;
    } else if (this.Column > 5) {
      this.Column = 1;
    }
    this.TopicHighlight.localPosition = new Vector3((float)(-375 + 125 * this.Column), (float)(375 - 125 * this.Row), this.TopicHighlight.localPosition.z);
    this.TopicSelected = (this.Row - 1) * 5 + this.Column;
    this.TopicNameLabel.text = ((!ConversationGlobals.GetTopicDiscovered(this.TopicSelected)) ? "??????????" : this.TopicNames[this.TopicSelected]);
  }

  // Token: 0x060001F2 RID: 498 RVA: 0x000280C4 File Offset: 0x000264C4
  private void DetermineOpinion() {
    int[] topics = this.JSON.Topics[7].Topics;
    this.Opinion = topics[this.TopicSelected];
  }

  // Token: 0x060001F3 RID: 499 RVA: 0x000280F4 File Offset: 0x000264F4
  private void UpdateTopics() {
    for (int i = 1; i < this.TopicIcons.Length; i++) {
      UISprite uisprite = this.TopicIcons[i];
      if (!ConversationGlobals.GetTopicDiscovered(i)) {
        uisprite.spriteName = 0.ToString();
        uisprite.color = new Color(uisprite.color.r, uisprite.color.g, uisprite.color.b, 0.5f);
      } else {
        uisprite.spriteName = i.ToString();
      }
    }
  }

  // Token: 0x060001F4 RID: 500 RVA: 0x00028198 File Offset: 0x00026598
  private void UpdateComplimentHighlight() {
    for (int i = 1; i < this.TopicIcons.Length; i++) {
      this.ComplimentBGs[this.ComplimentSelected].color = this.OriginalColor;
    }
    if (this.Line < 1) {
      this.Line = 5;
    } else if (this.Line > 5) {
      this.Line = 1;
    }
    if (this.Side < 1) {
      this.Side = 2;
    } else if (this.Side > 2) {
      this.Side = 1;
    }
    this.ComplimentSelected = (this.Line - 1) * 2 + this.Side;
    this.ComplimentBGs[this.ComplimentSelected].color = Color.white;
  }

  // Token: 0x060001F5 RID: 501 RVA: 0x0002825C File Offset: 0x0002665C
  private void UpdateTraitHighlight() {
    if (this.TraitSelected < 1) {
      this.TraitSelected = 3;
    } else if (this.TraitSelected > 3) {
      this.TraitSelected = 1;
    }
    for (int i = 1; i < this.TraitBGs.Length; i++) {
      this.TraitBGs[i].color = this.OriginalColor;
    }
    this.TraitBGs[this.TraitSelected].color = Color.white;
  }

  // Token: 0x060001F6 RID: 502 RVA: 0x000282D8 File Offset: 0x000266D8
  private void UpdateGiftHighlight() {
    for (int i = 1; i < this.GiftBGs.Length; i++) {
      this.GiftBGs[i].color = this.OriginalColor;
    }
    if (this.GiftRow < 1) {
      this.GiftRow = 2;
    } else if (this.GiftRow > 2) {
      this.GiftRow = 1;
    }
    if (this.GiftColumn < 1) {
      this.GiftColumn = 2;
    } else if (this.GiftColumn > 2) {
      this.GiftColumn = 1;
    }
    this.GiftSelected = (this.GiftRow - 1) * 2 + this.GiftColumn;
    this.GiftBGs[this.GiftSelected].color = Color.white;
  }

  // Token: 0x04000669 RID: 1641
  public StudentManagerScript StudentManager;

  // Token: 0x0400066A RID: 1642
  public InputManagerScript InputManager;

  // Token: 0x0400066B RID: 1643
  public LoveManagerScript LoveManager;

  // Token: 0x0400066C RID: 1644
  public PromptBarScript PromptBar;

  // Token: 0x0400066D RID: 1645
  public YandereScript Yandere;

  // Token: 0x0400066E RID: 1646
  public StudentScript Suitor;

  // Token: 0x0400066F RID: 1647
  public StudentScript Rival;

  // Token: 0x04000670 RID: 1648
  public PromptScript Prompt;

  // Token: 0x04000671 RID: 1649
  public JsonScript JSON;

  // Token: 0x04000672 RID: 1650
  public Transform AffectionSet;

  // Token: 0x04000673 RID: 1651
  public Transform OptionSet;

  // Token: 0x04000674 RID: 1652
  public GameObject HeartbeatCamera;

  // Token: 0x04000675 RID: 1653
  public GameObject SeductionIcon;

  // Token: 0x04000676 RID: 1654
  public GameObject PantyIcon;

  // Token: 0x04000677 RID: 1655
  public Transform TopicHighlight;

  // Token: 0x04000678 RID: 1656
  public Transform ComplimentSet;

  // Token: 0x04000679 RID: 1657
  public Transform AffectionBar;

  // Token: 0x0400067A RID: 1658
  public Transform Highlight;

  // Token: 0x0400067B RID: 1659
  public Transform GiveGift;

  // Token: 0x0400067C RID: 1660
  public Transform PeekSpot;

  // Token: 0x0400067D RID: 1661
  public Transform[] Options;

  // Token: 0x0400067E RID: 1662
  public Transform ShowOff;

  // Token: 0x0400067F RID: 1663
  public Transform Topics;

  // Token: 0x04000680 RID: 1664
  public Texture X;

  // Token: 0x04000681 RID: 1665
  public UITexture[] MultiplierIcons;

  // Token: 0x04000682 RID: 1666
  public UILabel[] ComplimentLabels;

  // Token: 0x04000683 RID: 1667
  public UISprite[] ComplimentBGs;

  // Token: 0x04000684 RID: 1668
  public UILabel MultiplierLabel;

  // Token: 0x04000685 RID: 1669
  public UILabel SeductionLabel;

  // Token: 0x04000686 RID: 1670
  public UILabel TopicNameLabel;

  // Token: 0x04000687 RID: 1671
  public UILabel DialogueLabel;

  // Token: 0x04000688 RID: 1672
  public UISprite[] TopicIcons;

  // Token: 0x04000689 RID: 1673
  public UIPanel DatingSimHUD;

  // Token: 0x0400068A RID: 1674
  public UILabel WisdomLabel;

  // Token: 0x0400068B RID: 1675
  public UISprite[] TraitBGs;

  // Token: 0x0400068C RID: 1676
  public UISprite[] GiftBGs;

  // Token: 0x0400068D RID: 1677
  public UITexture RoseIcon;

  // Token: 0x0400068E RID: 1678
  public UILabel[] Labels;

  // Token: 0x0400068F RID: 1679
  public UIPanel Panel;

  // Token: 0x04000690 RID: 1680
  public string[] Compliments;

  // Token: 0x04000691 RID: 1681
  public string[] TopicNames;

  // Token: 0x04000692 RID: 1682
  public string[] GiveGifts;

  // Token: 0x04000693 RID: 1683
  public string[] Greetings;

  // Token: 0x04000694 RID: 1684
  public string[] Farewells;

  // Token: 0x04000695 RID: 1685
  public string[] Negatives;

  // Token: 0x04000696 RID: 1686
  public string[] Positives;

  // Token: 0x04000697 RID: 1687
  public string[] ShowOffs;

  // Token: 0x04000698 RID: 1688
  public bool SelectingTopic;

  // Token: 0x04000699 RID: 1689
  public bool AffectionGrow;

  // Token: 0x0400069A RID: 1690
  public bool Complimenting;

  // Token: 0x0400069B RID: 1691
  public bool Matchmaking;

  // Token: 0x0400069C RID: 1692
  public bool GivingGift;

  // Token: 0x0400069D RID: 1693
  public bool ShowingOff;

  // Token: 0x0400069E RID: 1694
  public bool Negative;

  // Token: 0x0400069F RID: 1695
  public bool SlideOut;

  // Token: 0x040006A0 RID: 1696
  public bool Testing;

  // Token: 0x040006A1 RID: 1697
  public float HighlightTarget;

  // Token: 0x040006A2 RID: 1698
  public float Affection;

  // Token: 0x040006A3 RID: 1699
  public float Rotation;

  // Token: 0x040006A4 RID: 1700
  public float Speed;

  // Token: 0x040006A5 RID: 1701
  public float Timer;

  // Token: 0x040006A6 RID: 1702
  public int ComplimentSelected = 1;

  // Token: 0x040006A7 RID: 1703
  public int TraitSelected = 1;

  // Token: 0x040006A8 RID: 1704
  public int TopicSelected = 1;

  // Token: 0x040006A9 RID: 1705
  public int GiftSelected = 1;

  // Token: 0x040006AA RID: 1706
  public int Selected = 1;

  // Token: 0x040006AB RID: 1707
  public int AffectionLevel;

  // Token: 0x040006AC RID: 1708
  public int Multiplier;

  // Token: 0x040006AD RID: 1709
  public int Opinion;

  // Token: 0x040006AE RID: 1710
  public int Phase = 1;

  // Token: 0x040006AF RID: 1711
  public int GiftColumn = 1;

  // Token: 0x040006B0 RID: 1712
  public int GiftRow = 1;

  // Token: 0x040006B1 RID: 1713
  public int Column = 1;

  // Token: 0x040006B2 RID: 1714
  public int Row = 1;

  // Token: 0x040006B3 RID: 1715
  public int Side = 1;

  // Token: 0x040006B4 RID: 1716
  public int Line = 1;

  // Token: 0x040006B5 RID: 1717
  public string CurrentAnim = string.Empty;

  // Token: 0x040006B6 RID: 1718
  public Color OriginalColor;
}