using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pontuacao : MonoBehaviour
{
    public float maiorValor;
    public Text placar;

    void Start()
    {
        maiorValor = 0;
    }

    public void ReceberNovoValor(float valor)
    {
        if (valor > maiorValor)
        {
            maiorValor = valor;
            AtualizarPlacar();
        }
    }

    public void AtualizarPlacar()
    {
        placar.text = $"Pontuação: {maiorValor}";

    }
}
