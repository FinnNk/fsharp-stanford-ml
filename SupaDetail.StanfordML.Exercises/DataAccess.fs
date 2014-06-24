module DataAccess

open System.IO

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

let readCSV filename = 
    let convertRow (row: string) = row.Split(',') |> Seq.map float

    File.ReadLines filename
    |> Seq.map convertRow
    |> DenseMatrix.OfRows