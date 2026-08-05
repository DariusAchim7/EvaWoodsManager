using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Oferte;

public record LinieOfertaDto(
    Guid Id,
    Guid? ProdusId,
    string Descriere,
    CategorieProdus Categorie,
    UnitateMasura UM,
    decimal CantitateCalculata,
    decimal CantitateOfertata,
    decimal PretCost,
    decimal PretVanzare,
    decimal ProcentTva,
    decimal Total,
    decimal TotalCuTva,
    string? Nota,
    bool VizibilClient,
    bool EsteAutomat,
    decimal? PretCurentCatalog);

public record RezumatOfertaDto(
    decimal TotalMateriale,
    decimal TotalFeronerie,
    decimal TotalServicii,
    decimal Manopera,
    decimal CostTotal,
    decimal PretVanzareTotal,
    decimal ProfitEstimat,
    decimal MarjaProcent,
    decimal CostProduseFaraManopera);