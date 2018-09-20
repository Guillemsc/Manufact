using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageSelectionUI : MonoBehaviour
{
    private int curr_stage = 0;
    private int stage_to_change = 0;

    enum StageSelectionState
    {
        FADING_IN,
        WAITING_TO_FADE_OUT,
        FADING_OUT,
        CHANGE_STAGE_FADE_IN,
        CHANGE_STAGE_FADE_OUT,
        FINISHED,
    }

    private StageSelectionState state = StageSelectionState.FINISHED;

    [SerializeField] private float fade_in_time = 1.0f;
    private Timer fade_in_timer = new Timer();

    [SerializeField] private float fade_out_time = 1.0f;
    private Timer fade_out_timer = new Timer();

    [SerializeField] private float change_stage_time = 1.0f;
    private Timer change_stage_timer = new Timer();

    [SerializeField] private TMPro.TextMeshProUGUI stage_text = null;

    [SerializeField] private CanvasGroup canvas_group = null;
    [SerializeField] private Image background_image = null;
    [SerializeField] private Image change_state_image = null;
    [SerializeField] private Button next_stage_button = null;
    [SerializeField] private Button prev_stage_button = null;

    [SerializeField] private RectTransform scroll_view_rect = null;
    [SerializeField] private GameObject scroll_view_content = null;

    [SerializeField] private GameObject levels_prefab = null;

    [SerializeField] private float margins_x = 10.0f;
    [SerializeField] private float margins_y = 10.0f;
    [SerializeField] private float distance_x = 10.0f;
    [SerializeField] private float distance_y = 60.0f;

    private List<LevelButtonUI> levels_list = new List<LevelButtonUI>();

    private int to_update_positions = 0;

    private Vector2 scroll_view_size = Vector2.zero;

    private void Update()
    {
        CheckChangeScrollView();

        ActuallyUpdatePositions();

        UpdateState();
    }

    private void CheckChangeScrollView()
    {
        if(scroll_view_rect.rect.size != scroll_view_size)
        {
            scroll_view_size = scroll_view_rect.rect.size;

            UpdateLevelsPosition();
            UpdateLevelsPosition();
        }
    }

    private void ActuallyUpdatePositions()
    {
        if (to_update_positions > 0)
        {
            int amount_per_line = 0;
            float actual_distance_x = distance_x;

            Vector2 size = levels_list[0].GetSize();

            if (levels_list.Count > 0)
            {
                if ((size.x + distance_x) != 0)
                {
                    float disponible_space = scroll_view_rect.rect.size.x - (margins_x * 2);

                    float amount_per_line_dec = (disponible_space + distance_x) / (size.x + distance_x);

                    amount_per_line = (int)Mathf.Floor(amount_per_line_dec);

                    float used_space = ((float)amount_per_line * (float)size.x) + ((float)(amount_per_line - 1) * (float)distance_x);

                    if ((amount_per_line - 1) != 0)
                    {
                        float space_to_add = (scroll_view_rect.rect.size.x - (margins_x * 2)) - used_space;
                        space_to_add /= (amount_per_line - 1);

                        actual_distance_x = size.x + distance_x + space_to_add;
                    }
                }
            }

            int curr_row = 0;
            int curr_column = 0;

            for (int i = 0; i < levels_list.Count; ++i)
            {
                LevelButtonUI curr_button = levels_list[i];

                curr_button.gameObject.transform.localScale = new Vector3(1, 1, 1);
                curr_button.gameObject.transform.parent = scroll_view_content.transform;

                Vector2 new_position = new Vector2(margins_x, -margins_y);
                new_position.x += size.x / 2;
                new_position.y -= size.y / 2;

                new_position.x += curr_row * actual_distance_x;
                new_position.y -= curr_column * (distance_y + size.y);

                curr_button.transform.localPosition = new_position;

                if (curr_row + 1 >= amount_per_line)
                {
                    curr_row = 0;
                    ++curr_column;
                }
                else
                    ++curr_row;
            }

            scroll_view_rect.sizeDelta = new Vector2(scroll_view_rect.sizeDelta.x, (margins_y * 2) + ((size.y + distance_y) * curr_column));

            --to_update_positions;
        }
    }

    private void SetStageGameObjects(int stage)
    {
        curr_stage = stage;

        UpdateNextPreviousButtons();

        stage_text.text = LocManager.Instance.GetText("Stage") + ": " + stage;

        for (int i = 0; i < levels_list.Count; ++i)
        {
            Destroy(levels_list[i].gameObject);   
        }

        levels_list.Clear();

        int to_spawn = LevelsManager.Instance.GetStageLevelsCount(stage);

        for(int i = 0; i < to_spawn; ++i)
        {
            GameObject new_go = Instantiate(levels_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            LevelButtonUI button_ui = new_go.GetComponent<LevelButtonUI>();

            if (button_ui != null)
            {
                button_ui.SetLevel(curr_stage, i);
                levels_list.Add(button_ui);
            }
        }

        Canvas.ForceUpdateCanvases();

        UpdateLevelsPosition();
        UpdateLevelsPosition();
    }

    private void UpdateLevelsPosition()
    {
        ++to_update_positions;
    }

    private void UpdateState()
    {
        switch(state)
        {
            case StageSelectionState.FADING_IN:
                if(fade_in_timer.ReadTime() > fade_in_time)
                {
                    state = StageSelectionState.WAITING_TO_FADE_OUT;
                }
                break;
            case StageSelectionState.WAITING_TO_FADE_OUT:
                break;
            case StageSelectionState.FADING_OUT:
                if(fade_out_timer.ReadTime() > fade_out_time)
                {
                    gameObject.SetActive(false);
                    state = StageSelectionState.FINISHED;
                }
                break;
            case StageSelectionState.CHANGE_STAGE_FADE_IN:
                if(change_stage_timer.ReadTime() > change_stage_time * 0.5f)
                {
                    SetStageGameObjects(stage_to_change);

                    change_state_image.DOFade(0, change_stage_time * 0.5f);

                    state = StageSelectionState.CHANGE_STAGE_FADE_OUT;
                }
                break;
            case StageSelectionState.CHANGE_STAGE_FADE_OUT:
                if (change_stage_timer.ReadTime() > change_stage_time)
                {
                    change_state_image.gameObject.SetActive(false);
                    state = StageSelectionState.WAITING_TO_FADE_OUT;
                }
                break;
            case StageSelectionState.FINISHED:
                break;
        }
    }

    public void FadeIn(int starting_stage = 0)
    {
        if (state != StageSelectionState.FADING_OUT)
        {
            gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();

            SetStageGameObjects(starting_stage);

            state = StageSelectionState.FADING_IN;

            Canvas.ForceUpdateCanvases();

            Vector3 starting_pos = new Vector3(canvas_group.gameObject.transform.position.x + (background_image.rectTransform.rect.width * 0.6f),
                canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

            background_image.gameObject.transform.position = starting_pos;

            background_image.transform.DOMoveX(canvas_group.gameObject.transform.position.x, fade_in_time);

            change_state_image.color = new Color(change_state_image.color.r, change_state_image.color.g, change_state_image.color.b, 0);

            change_state_image.gameObject.SetActive(false);

            fade_in_timer.Start();
        }
    }

    public void FadeOut()
    {
        if(state == StageSelectionState.WAITING_TO_FADE_OUT)
        {
            Vector3 finish_pos = new Vector3(canvas_group.gameObject.transform.position.x - (background_image.rectTransform.rect.width * 0.6f),
            canvas_group.gameObject.gameObject.transform.position.y, canvas_group.gameObject.transform.position.z);

            background_image.transform.DOMoveX(finish_pos.x, fade_out_time);

            fade_out_timer.Start();

            state = StageSelectionState.FADING_OUT;
        }
    }

    public void NextStage()
    {
        LevelsManager.LevelsStage stage = LevelsManager.Instance.GetLevelStage(curr_stage + 1);

        if(stage != null)
        {
            state = StageSelectionState.CHANGE_STAGE_FADE_IN;
            change_stage_timer.Start();

            stage_to_change = curr_stage + 1;

            change_state_image.gameObject.SetActive(true);
            change_state_image.DOFade(1, change_stage_time * 0.5f);
        }
    }

    public void PreviousStage()
    {
        LevelsManager.LevelsStage stage = LevelsManager.Instance.GetLevelStage(curr_stage - 1);

        if (stage != null)
        {
            state = StageSelectionState.CHANGE_STAGE_FADE_IN;
            change_stage_timer.Start();

            stage_to_change = curr_stage - 1;

            change_state_image.gameObject.SetActive(true);
            change_state_image.DOFade(1, change_stage_time * 0.5f);
        }
    }

    private void UpdateNextPreviousButtons()
    {
        LevelsManager.LevelsStage next_stage = LevelsManager.Instance.GetLevelStage(curr_stage + 1);

        if (next_stage == null)
            next_stage_button.gameObject.SetActive(false);
        else
            next_stage_button.gameObject.SetActive(true);

        LevelsManager.LevelsStage prev_stage = LevelsManager.Instance.GetLevelStage(curr_stage - 1);

        if (prev_stage == null)
            prev_stage_button.gameObject.SetActive(false);
        else
            prev_stage_button.gameObject.SetActive(true);
    }
}
