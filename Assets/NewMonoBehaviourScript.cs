using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.Json;
using UnityEngine.UIElements;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public List<CharacterAllStatement> CharacterAllLeftSideStatements = new List<CharacterAllStatement>();
    public List<CharacterAllStatement> CharacterAllRightSideStatements = new List<CharacterAllStatement>();

    void Start()
    {
        StreamWriter LeftWriter = new StreamWriter("Assets/Hitbox.txt/LeftSideHitbox.txt");
        StreamWriter RightWriter = new StreamWriter("Assets/Hitbox.txt/RightSideHitbox.txt");

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            CharacterAllStatement lcas = new CharacterAllStatement();
            lcas.Statement = child.name;

            CharacterAllStatement rcas = new CharacterAllStatement();
            rcas.Statement = child.name;

            List<FrameData> LeftFrames = new List<FrameData>();
            List<FrameData> RightFrames = new List<FrameData>();

            for (int j = 0; j < child.childCount; j++)
            {
                LeftFrames.Add(GetLeftSideFrameData(child.GetChild(j)));
                RightFrames.Add(GetRightSideFrameData(child.GetChild(j)));
            }

            lcas.FrameData = LeftFrames;
            CharacterAllLeftSideStatements.Add(lcas);
            rcas.FrameData = RightFrames;
            CharacterAllRightSideStatements.Add(rcas);
        }

        LeftWriter.Write(JsonSerializer.Serialize(CharacterAllLeftSideStatements));
        LeftWriter.Flush();
        LeftWriter.Close();

        RightWriter.Write(JsonSerializer.Serialize(CharacterAllRightSideStatements));
        RightWriter.Flush();
        RightWriter.Close();
    }
    
    private FrameData GetRightSideFrameData(Transform transform)
    {
        int frameNumber = int.Parse(transform.name);
        float scale = 1.5f;

        // 원래 중심 위치
        Vector3 originalCenter = transform.GetChild(0).localPosition;
        float[] center = new float[] { originalCenter.x * scale, originalCenter.y * scale };

        List<HurtBox> hurtBoxes = new List<HurtBox>();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Transform part = transform.GetChild(0).GetChild(i);
            BoxCollider boxCollider = part.GetComponent<BoxCollider>();

            // 1. 박스 중심을 월드 좌표로 변환
            Vector3 worldBoxCenter = part.TransformPoint(boxCollider.center);

            // 2. 확대 후 중심 기준으로 좌우 반전
            Vector3 scaledWorldCenter = (worldBoxCenter - transform.position) * scale + transform.position;

            // 좌우 반전 (중심 기준 대칭)
            float mirroredX = 2 * transform.position.x - scaledWorldCenter.x;
            Vector3 mirroredWorldCenter = new Vector3(mirroredX, scaledWorldCenter.y, scaledWorldCenter.z);

            // 3. 다시 transform 로컬로 변환
            Vector3 localOffset = transform.InverseTransformPoint(mirroredWorldCenter);

            // 4. 크기 확대
            Vector3 scaledSize = boxCollider.size * scale;

            // 저장
            HurtBox hurtBox = new HurtBox();
            hurtBox.PartName = part.name;
            hurtBox.OffSet = new float[] { localOffset.x, localOffset.y };
            hurtBox.Size = new float[] { scaledSize.x, scaledSize.y };
            hurtBoxes.Add(hurtBox);
        }

        return new FrameData(frameNumber, center, hurtBoxes);
    }


    private FrameData GetLeftSideFrameData(Transform transform)
    {
        int frameNumber = int.Parse(transform.name);
        float scale = 1.5f;

        // 중심: 원래 로컬 중심
        Vector3 originalCenter = transform.GetChild(0).localPosition;

        float[] center = new float[2] { originalCenter.x * scale, originalCenter.y * scale };
        List<HurtBox> hurtBoxes = new List<HurtBox>();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Transform part = transform.GetChild(0).GetChild(i);
            BoxCollider boxCollider = part.GetComponent<BoxCollider>();

            // 1. 현재 박스 중심의 '월드 위치' 계산
            Vector3 worldBoxCenter = part.TransformPoint(boxCollider.center);

            // 2. 중심 기준으로 스케일 확대
            Vector3 scaledWorldCenter = (worldBoxCenter - transform.position) * scale + transform.position;

            // 3. 다시 transform 기준 로컬 위치로 변환
            Vector3 localOffset = transform.InverseTransformPoint(scaledWorldCenter);

            // 4. 박스 크기 확대
            Vector3 scaledSize = boxCollider.size * scale;

            // 저장
            HurtBox hurtBox = new HurtBox();
            hurtBox.PartName = part.name;
            hurtBox.OffSet = new float[] { localOffset.x, localOffset.y };
            hurtBox.Size = new float[] { scaledSize.x, scaledSize.y };
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
        public FrameData(int frameNumber, float[] center, List<HurtBox> hurtBoxes)
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