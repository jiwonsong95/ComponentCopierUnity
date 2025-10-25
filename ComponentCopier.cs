using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ComponentCopier : EditorWindow
{
    [SerializeField] private Component source;
    [SerializeField] private Component target;

    [MenuItem("Tools/Component Field Copier")]
    private static void Open()
    {
        GetWindow<ComponentCopier>("Component Field Copier");
    }

    private void OnGUI()
    {
        GUILayout.Label("🎯 Component Field Copier", EditorStyles.boldLabel);

        source = (Component)EditorGUILayout.ObjectField("Source Component", source, typeof(Component), true);
        target = (Component)EditorGUILayout.ObjectField("Target Component", target, typeof(Component), true);

        GUI.enabled = source && target;
        if (GUILayout.Button("Copy Common Fields"))
        {
            CopyCommonFields(source, target);
        }
        GUI.enabled = true;
    }

    private void CopyCommonFields(Component src, Component dst)
    {
        var srcType = src.GetType();
        var dstType = dst.GetType();
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        int count = 0;

        // 타겟(dst)의 타입을 기준으로 부모 클래스를 순회합니다.
        var currentDstType = dstType;
        while (currentDstType != null && currentDstType != typeof(MonoBehaviour) && currentDstType != typeof(Component))
        {
            // DeclaredOnly를 사용해 현재 레벨에 선언된 필드만 가져옵니다.
            var dstFields = currentDstType.GetFields(flags | BindingFlags.DeclaredOnly);

            foreach (var dstField in dstFields)
            {
                if (dstField.IsStatic || dstField.IsInitOnly) continue;

                // 소스 타입(srcType)에서 이름이 같은 필드를 찾습니다.
                // GetField는 부모 클래스까지 모두 검색합니다.
                var srcField = srcType.GetField(dstField.Name, flags);

                // 소스에 해당 필드가 없거나 타입이 다르면 건너뜁니다.
                if (srcField == null) continue;
                if (srcField.FieldType != dstField.FieldType) continue;

                // 값 복사
                try
                {
                    var value = srcField.GetValue(src);
                    dstField.SetValue(dst, value);
                    count++;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"⚠️ 필드 '{dstField.Name}' 복사 실패: {e.Message}");
                }
            }

            // 다음 부모 클래스로 이동
            currentDstType = currentDstType.BaseType;
        }

        if (count > 0)
        {
            EditorUtility.SetDirty(dst);
            Debug.Log($"✅ {count}개 필드를 {srcType.Name} → {dstType.Name} 로 복사 완료!");
        }
        else
        {
            Debug.LogWarning($"⚠️ 복사할 공통 필드를 찾지 못했습니다. ({srcType.Name} → {dstType.Name})");
        }
    }
}
