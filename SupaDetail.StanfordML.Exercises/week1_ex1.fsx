#r """..\packages\MathNet.Numerics.3.0.0-beta05\lib\net40\MathNet.Numerics.dll"""
#r """..\packages\MathNet.Numerics.FSharp.3.0.0-beta05\lib\net40\MathNet.Numerics.FSharp.dll"""
#load "../packages/FSharp.Charting.0.90.6/FSharp.Charting.fsx"
#load "DataAccess.fs"
#load "LinearRegression.fs"

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open FSharp.Charting
open System.Drawing

open System.IO

open LinearRegression

// 1 warmup exercise
let ones = DenseMatrix.CreateIdentity(5)

let data = DataAccess.readCSV (__SOURCE_DIRECTORY__ + @"\ex1data1.txt")

let data' = (addIntercept data) :?> DenseMatrix

let X = data'.[*, 0..1]
let y = data'.[*, 2]

let iterations = 1500
let alpha = 0.01
let theta0 = vector [0.; 0.]

let (theta, history) = gradientDescent X y theta0 alpha iterations

Chart.Combine[
    Seq.zip X.[*,1] y
        |> Chart.Point
        |> Chart.WithMarkers(Style = ChartTypes.MarkerStyle.Cross, Color = Color.Red, Size=10)

    Seq.zip X.[*,1] (X * theta)
        |> Chart.Line
]
|> Chart.WithXAxis(Title = "Population of City in 10,000s")
|> Chart.WithYAxis(Title = "Profit in $10,000s")