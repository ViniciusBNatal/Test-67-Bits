using UnityEngine;

public class PlayerModel : MonoSingleton<PlayerModel>
{
    private Material[] _materials;

    protected override void Awake()
    {
        base.Awake();
        SkinnedMeshRenderer[] skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
        _materials = new Material[skinneds.Length];
        for (int i = 0; i < skinneds.Length; i++)
        {
            _materials[i] = skinneds[i].material;
        }
    }

    public void UpdateColor(Color color)
    {
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = color;
        }
    }
}
