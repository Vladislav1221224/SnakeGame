using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Автор: Твоє Ім'я
/// Дата створення: #DATE#
/// </summary>
public class ColliderTrigger : MonoBehaviour
{
    #region Properties

    #region UnityEvents

    public UnityAction<Collider2D> OnTriggerEnter;

    #endregion

    #endregion

    #region Methods

    #region Unity methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }

    #endregion

    #endregion
}