using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState<entity_type>
{
  void Enter(entity_type entity);
  void Execute(entity_type entity);
  void Exit(entity_type entity);
  bool OnMessage(entity_type entity,Telegram telegram);
}