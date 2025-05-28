using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.Json;
using UnityEngine.UIElements;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public List<CharacterAllStatement> CharacterAllStatements = new List<CharacterAllStatement>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            CharacterAllStatement cas = new CharacterAllStatement();
            cas.Statement = child.name;

            List<FrameData> frames = new List<FrameData>();
            for (int j = 0; j < child.childCount; j++)
            {
                frames.Add(GetFrameData(child.GetChild(j)));
            }

            cas.FrameData = frames;
            CharacterAllStatements.Add(cas);
        }

        StreamWriter writer = new StreamWriter("Assets/text.txt");
        writer.Write(JsonSerializer.Serialize(CharacterAllStatements));
        writer.Flush();
        writer.Close();
    }

    private FrameData GetFrameData(Transform transform)
    {
        int frameNumber = int.Parse(transform.name);
        float[] center = new float[2];
        List<HurtBox> hurtBoxes = new List<HurtBox>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Debug.Log(transform.GetChild(0).localPosition + " " + transform.GetChild(0).name);
            Vector2 position = transform.GetChild(0).localPosition;
            center[0] = position.x;
            center[1] = position.y;
            HurtBox hurtBox = new HurtBox();
            hurtBox.PartName = transform.GetChild(0).GetChild(i).name;
            BoxCollider boxCollider = transform.GetChild(0).GetChild(i).GetComponent<BoxCollider>();
            hurtBox.OffSet = new float[] { boxCollider.center.x + position.x, boxCollider.center.y + position.y };
            hurtBox.Size = new float[] { boxCollider.size.x, boxCollider.size.y };
            hurtBoxes.Add(hurtBox);
        }
        
        return new FrameData(frameNumber, center, hurtBoxes);
    }

    void Update()
    {
    }

    public class HurtBox
    {
        public string PartName { get; set; }
        public float[] OffSet { get; set; }
        public float[] Size { get; set; }
    }

    public class FrameData
    {
        public FrameData(int frameNumber,float[] center, List<HurtBox> hurtBoxes)
        {
            FrameNumber = frameNumber;
            Center = center;
            HurtBoxes = hurtBoxes;
        }

        public int FrameNumber { get; set; }
        public float[] Center { get; set; }
        public List<HurtBox> HurtBoxes { get; set; }
    }

    public class CharacterAllStatement
    {
        public string Statement { get; set; }
        public List<FrameData> FrameData { get; set; }
    }
}