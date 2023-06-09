﻿using Microsoft.AspNetCore.Mvc;
using w_escolas.Infra.Data;
using w_escolas.Shared;

namespace w_escolas.Endpoints.Turmas;

public class TurmaDelete
{
    public static string Template => "/Turmas/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(
        [FromRoute] Guid id,
        ApplicationDbContext context,
        UserInfo userInfo)
    {
        if (context.Turmas == null)
            return Results.UnprocessableEntity();

        var escolaIdDoUsuarioCorrente = userInfo.GetEscolaId();

        var turma = context.Turmas.Where(e => e.Id == id).FirstOrDefault();
        if (turma == null)
            return Results.NotFound();

        if (turma.EscolaId != escolaIdDoUsuarioCorrente)
            return Results.ValidationProblem(
                "Não é proprietário da escola".ConvertToProblemDetails()
            );

        //if (NaoPodeExcluir(context, id))
        //    return Results.ValidationProblem(errorMessages.ConvertToProblemDetails());

        context.Turmas.Remove(turma);
        context.SaveChanges();

        return Results.Ok();
    }

    //private static readonly List<string> errorMessages = new();
    //private static void TemAlunoVinculado(ApplicationDbContext context, Guid cursoId)
    //{
    //    if (context.Turmas.Where(t => t.CursoId == cursoId).Any())
    //        errorMessages.Add("Existem Turmas(s) vinculada(s)");
    //}
    //private static bool NaoPodeExcluir(ApplicationDbContext context, Guid cursoId)
    //{
    //    errorMessages.Clear();
    //    TemTurmaVinculada(context, cursoId);
    //    return errorMessages.Count > 0;
    //}
}
