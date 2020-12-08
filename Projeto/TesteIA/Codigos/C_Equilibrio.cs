using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UI;

public class C_Equilibrio : Agent
{
    public Slider slider;
    public GameObject placar;
    public float pontuacao;

    public float PosMax = 10;
    public float PosAtu;

    public float forca = 1;
    public float impulcao;
    public float bonusDif = 0;

    float porcent;

    bool perdeu;

    // Start is called before the first frame update
    void Start()
    {
        perdeu = false;
        PosAtu = 0;

        slider.maxValue = PosMax;
        slider.minValue = -PosMax;
    }

    // Update is called once per frame
    void Update()
    {
        AtuVal();
        Impulcionar();
        RequestDecision();
        //Controles();

        // Fim
        ContJogo();

    }

    ////////////////////////////////////

    public event Action OnReset;

    public override void Initialize()
    {
        PosAtu = 0;
    }

    // Aprendizagem
    public override void Heuristic(float[] actionsOut)
    {

        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 0;
        }
    }

    // Coletar Observacoes
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(PosAtu);
    }

    // Metodo para executar acoes
    public override void OnActionReceived(float[] vectorAction)
    {
        // Acoes para decisoes
        if (vectorAction[0] == 0)
        {
            IrParaEsquerda();
        }
        if (vectorAction[0] == 1)
        {
            IrParaDireita();
        }

        // Verifica a situação de Rewards
        if(perdeu)
        {
            //Debug.Log("Perdeu");
            perdeu = false;
            AddReward(-1.0f);
            EndEpisode();
        }
        else if(PosAtu > -3 && PosAtu < 3)
        {
            AddReward(0.1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    void ContJogo()
    {
        if(PosAtu > -PosMax && PosAtu < PosMax)
        {
            pontuacao += 1;// Time.deltaTime;
        }
        else
        {
            placar.GetComponent<Pontuacao>().ReceberNovoValor(pontuacao);
            perdeu = true;
        }
    }

    // Resetar tudo
    public void Reset()
    {
        PosAtu = 0;
        pontuacao = 0;
        bonusDif = 0;
        impulcao = 0;
    }


    // Funcao para ir para esquerda
    public void IrParaEsquerda()
    {
        //Debug.Log("Esq");
        PosAtu -= 5;// * Time.deltaTime;
    }

    // Funcao para ir para direita
    public void IrParaDireita()
    {
        //Debug.Log("Dir");
        PosAtu += 5;// * Time.deltaTime;
    }

    /////////////////////////////////////
    void Controles()
    {
        if(Input.GetKey(KeyCode.A))
        {
            IrParaEsquerda();
        }
        if (Input.GetKey(KeyCode.D))
        {
            IrParaDireita();
        }
    }

    void Impulcionar()
    {
        porcent = PosAtu / PosMax;

        //if (porcent <= .25f)
        //{
        //    impulcao += (-forca + forca / 10) * Time.deltaTime;
        //}
        //else if (porcent <= .50f)
        //{
        //    impulcao += (-forca - forca / 5) * Time.deltaTime;
        //}
        //else if (porcent <= .75f)
        //{
        //    impulcao -= -(forca - forca / 5) * Time.deltaTime;
        //}
        //else
        //{
        //    impulcao -= -(forca - forca / 10) * Time.deltaTime;
        //}

        //if (impulcao > forca * 3)
        //{
        //    impulcao = forca - forca / 10;
        //}
        //else if (impulcao < -forca * 3)
        //{
        //    impulcao = -(forca - forca / 10);
        //}

        //bonusDif += Time.deltaTime * 2 * Time.deltaTime;

        //PosAtu += (impulcao + bonusDif) * Time.deltaTime;
        


        if (PosAtu < 0)
        {
            PosAtu += (-1 + porcent);// * Time.deltaTime;
            //Debug.Log((porcent) );
        }
        else if (PosAtu >= 0)
        {
            PosAtu += (1 + porcent);// * Time.deltaTime;
            //Debug.Log((porcent) );
        }
    }

    // Atualizar Slider
    void AtuVal()
    {
        slider.value = PosAtu * 10 / PosMax;
    }
}
