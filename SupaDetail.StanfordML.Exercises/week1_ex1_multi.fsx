#r """..\packages\MathNet.Numerics.3.0.0-beta05\lib\net40\MathNet.Numerics.dll"""
#r """..\packages\MathNet.Numerics.FSharp.3.0.0-beta05\lib\net40\MathNet.Numerics.FSharp.dll"""
#load "DataAccess.fs"
#load "LinearRegression.fs"

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics.Statistics
open MathNet.Numerics.LinearRegression

open System.IO

open LinearRegression

let data = DataAccess.readCSV (__SOURCE_DIRECTORY__ + "\\ex1data2.txt")

let X = data.[*, 0..data.ColumnCount-2]
let y = data.[*, data.ColumnCount-1]

let XNorm = 
    X
        |> featureNormalise
        |> addIntercept

let alpha = 0.01
let num_iters = 400
let theta0 = DenseVector.Create(3, 0.)

let (theta, history) = gradientDescent XNorm y theta0 alpha num_iters


// 7 normal equations
let X' = X |> addIntercept

let theta' = MultipleRegression.NormalEquations(X', y) // math.net

let theta'' = ((X'.Transpose() * X').Inverse() * X'.Transpose()) * y // manual