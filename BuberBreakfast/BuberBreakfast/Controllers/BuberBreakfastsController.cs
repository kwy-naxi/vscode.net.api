using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using BuberBreakfast.ServicesErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastsController : IApiController
{
  private readonly IBreakfastService _breakfastService;

  public BreakfastsController(IBreakfastService breakfastService)
  {
    _breakfastService = breakfastService;
  }

  [HttpPost()]
  public IActionResult CreateBreakfast(CreateBreakfastRequest request)
  {
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);

    if (requestToBreakfastResult.IsError)
    {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

    return CreatedAtAction(
      actionName: nameof(GetBreakfast),
      routeValues: new { id = breakfast.Id },
      value: MapBreakfastResponse(breakfast));
  }

  [HttpGet("{id:guid}")]
  public IActionResult GetBreakfast(Guid id)
  {
    ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

    return getBreakfastResult.Match(
      breakfast => Ok(MapBreakfastResponse(breakfast)),
      errors => Problem(errors));
  }

  [HttpPut("{id:guid}")]
  public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
  {
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);

    if (requestToBreakfastResult.IsError)
    {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<UpsertedBreakfast> upsertedResult = _breakfastService.UpsertBreakfast(breakfast);

    return NoContent();
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeleteBreakfast(Guid id)
  {
    ErrorOr<Deleted> deleteResult = _breakfastService.DeleteBreakfast(id);

    return deleteResult.Match(
      deleted => NoContent(),
      errors => Problem(errors));
  }

  private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
  {
    return new BreakfastResponse(
      breakfast.Id,
      breakfast.Name,
      breakfast.Description,
      breakfast.StartDateTime,
      breakfast.EndDateTime,
      breakfast.LastModifiedDateTime,
      breakfast.Savory,
      breakfast.Sweet
    );
  }
}