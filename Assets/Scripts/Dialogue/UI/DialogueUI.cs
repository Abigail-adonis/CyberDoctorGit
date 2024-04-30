using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using CyberDoctor.Dialogue;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image faceRight,faceLeft;
    public Text nameRight,nameLeft;
    public GameObject continueBox;
    private DialoguePiece currentPiece;

    public GameObject optionButtonPrefab; // 选项按钮的预制件
    public Transform optionsPanel; // 用于放置选项按钮的面板

    private bool shouldListenForClick = false;
    public event Action<DialogueOption> OnOptionSelected;
    private void Awake()
    {
        continueBox.SetActive(false);
    }

    private void OnEnable()
    {
        // 确保主摄像机上存在MouseInput组件
        if (Camera.main.GetComponent<MouseInput>() == null)
        {
            Camera.main.gameObject.AddComponent<MouseInput>();
        }
        EventHandlers.ShowDialogueEvent += OnShowDialogueEvent;
        EventHandlers.OnShowOptionsEvent += ShowOptions; // 添加这行来订阅选项显示事件
        MouseInput.OnMouseDown += OnMouseDown;
    }

    private void OnDisable()
    {
        EventHandlers.ShowDialogueEvent -= OnShowDialogueEvent;
        EventHandlers.OnShowOptionsEvent -= ShowOptions; // 取消订阅
        MouseInput.OnMouseDown -= OnMouseDown;
    }
    

    // 鼠标点击事件的处理
    private void OnMouseDown()
    {
        if (shouldListenForClick && currentPiece != null)
        {
            currentPiece.isDone = true;
            shouldListenForClick = false; // 用户已经点击，不再监听点击
            continueBox.SetActive(false); // 隐藏继续提示
        }
    }


    private void OnShowDialogueEvent(DialoguePiece piece)
    {
        currentPiece = piece;
        StartCoroutine(ShowDialogue(piece));
        /*if (piece.options != null && piece.options.Count > 0)
        {
            ShowOptions(piece.options);
        }*/
    }

    private void ShowOptions(List<DialogueOption> options)
    {
        // 先清除所有现有的选项按钮
        foreach (Transform child in optionsPanel)
        {
            Destroy(child.gameObject);
        }

        // 为每个选项创建一个按钮
        foreach (var option in options)
        {
            /*GameObject buttonGO = Instantiate(optionButtonPrefab, optionsPanel.position, Quaternion.identity, optionsPanel);
            Button button = buttonGO.GetComponent<Button>();
        
            // 设置按钮的文本
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            buttonText.text = option.text;
    
            // 为按钮添加点击事件监听器
            button.onClick.AddListener(() => OnOptionSelected(option.onSelectEvent));
            // 添加点击事件监听器*/
            GameObject buttonObject = Instantiate(optionButtonPrefab, optionsPanel);
            buttonObject.GetComponentInChildren<Text>().text = option.text;
            buttonObject.GetComponent<Button>().onClick.AddListener(() => SelectOption(option));
        }
        // 当选项出现时，暂停游戏
        Time.timeScale = 0f;
    }

    private void SelectOption(DialogueOption option)
    {
        currentPiece.selectedOption = option;
        currentPiece.isDone = true; // 可以标记对话片段为完成，或者根据需要调整逻辑
    } 
    private void OptionSelected(DialogueOption option)
    {
        ClearOptions();  // 清除所有选项按钮
        EventHandlers.RaiseOptionSelected(option);  // 触发全局事件
    }  

    private void ClearOptions()
    {
        foreach (Transform child in optionsPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private IEnumerator ShowDialogue(DialoguePiece piece)
    {
         if (piece != null)
        {
            piece.isDone = false;

            dialogueBox.SetActive(true);
            continueBox.SetActive(false);
            
            dialogueText.text = string.Empty;

            if (piece.name != string.Empty)//名字不为空
            {
                if (piece.onLeft)//头像在左
                {
                    faceRight.gameObject.SetActive(true);//关闭右边的
                    faceRight.gameObject.GetComponent<Image>().color = Color.gray;
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.gameObject.GetComponent<Image>().color = Color.white;
                    faceLeft.sprite = piece.faceImage;
                    nameLeft.text = piece.name;
                }
                else//头像在右
                {
                    faceRight.gameObject.SetActive(true);
                    faceRight.gameObject.GetComponent<Image>().color = Color.white;
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.gameObject.GetComponent<Image>().color = Color.gray;
                    faceRight.sprite = piece.faceImage;
                    nameRight.text = piece.name;
                }
            }
            else//名字都为空
            {
                faceLeft.gameObject.SetActive(false);
                faceRight.gameObject.SetActive(false);
                //nameLeft.gameObject.SetActive(false);
                //nameRight.gameObject.SetActive(false);
            }
            yield return dialogueText.DOText(piece.dialogueText, 1f).WaitForCompletion();//等候显示文字

            if (piece.hasToPause) // 如果需要暂停
            {
                continueBox.SetActive(true); 
                shouldListenForClick = true; // 开始监听鼠标点击
                while (!piece.isDone) // 等待用户点击鼠标
                {
                    yield return null; // 等待下一帧
                }
            }
            else if (piece.options != null && piece.options.Count > 0)
            {
                // 如果有选项，显示
                ShowOptions(piece.options);
            }
            else
            {
                shouldListenForClick = false; // 不需要暂停时，不监听鼠标点击
                piece.isDone = true; // 如果不需要暂停，则直接设置为完成
            }
        }
        else
        {
            dialogueBox.SetActive(false);
            yield break;
        }
    }
}
