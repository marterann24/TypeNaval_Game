using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public RuntimeAnimatorController[] fondosPorNivel;  // 4 animator controllers
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        int nivel = TypingManager.nivelActual;
        
        if (anim == null || fondosPorNivel == null || nivel < 0 || nivel >= fondosPorNivel.Length)
            return;

        RuntimeAnimatorController fondo = fondosPorNivel[nivel];

        if (fondo != null && fondo.animationClips.Length > 0)
        {
            anim.runtimeAnimatorController = fondo;
        }
    }
}
