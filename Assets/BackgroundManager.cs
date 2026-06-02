using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public RuntimeAnimatorController[] fondosPorNivel;  // 4 animaciones
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        int nivel = TypingManager.nivelActual;
        
        if (nivel < fondosPorNivel.Length)
        {
            anim.runtimeAnimatorController = fondosPorNivel[nivel];
        }
    }
}