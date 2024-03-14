module private HouseholdManager.Cookbook.Domain

open System

type Recipe =
    { Id: Guid
      Title: string
      Description: string }