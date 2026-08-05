using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Oferte;

public record AdaugaLinieOfertaRequest(
    Guid ProiectId,
    Guid? ProdusId,
    string Descriere,
    CategorieProdus Categorie,
    UnitateMasura UM,
    decimal CantitateOfertata,
    decimal PretCost,
    decimal PretVanzare,
    decimal ProcentTva = 21m,
    bool VizibilClient = true,
    string? Nota = null);