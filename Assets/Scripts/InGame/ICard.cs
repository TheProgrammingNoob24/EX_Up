using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ICard
{
    // �J�[�h�����X�R�A��ϓ�������l
    int UpdateScoreValue { get; }

    /// <summary>
    /// updateScoreValue����ɃX�R�A��ϓ�������
    /// </summary>
    public void updateScore();

}
