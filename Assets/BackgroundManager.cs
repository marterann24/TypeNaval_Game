using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public RuntimeAnimatorController[] fondosPorNivel;  // 4 animator controllers
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        int nivel = TypingManager.nivelActual;
        
        if (fondosPorNivel != null && nivel < fondosPorNivel.Length)
        {
            anim.runtimeAnimatorController = fondosPorNivel[nivel];
        }
    }
}