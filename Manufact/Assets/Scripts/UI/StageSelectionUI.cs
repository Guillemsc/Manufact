using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectionUI : MonoBehaviour
{
    private int curr_stage = 0;

    [SerializeField] TMPro.TextMeshProUGUI stage_text = null;

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

            for (int i = 0; i < 100; ++i)
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

        stage_text.text = LocManager.Instance.GetText("Stage") + ": " + stage;

        for (int i = 0; i < levels_list.Count; ++i)
        {
            Destroy(levels_list[i]);   
        }

        levels_list.Clear();

        int to_spawn = LevelsManager.Instance.GetStageLevelsCount(stage);

        for(int i = 0; i < 100; ++i)
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

    public void FadeIn(int starting_stage = 0)
    {
        gameObject.SetActive(true);

        SetStageGameObjects(starting_stage);
    }
}
