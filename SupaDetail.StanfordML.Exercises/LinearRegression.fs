module LinearRegression

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics.Statistics
open MathNet.Numerics.LinearRegression

let vector =  List.ofSeq >> vector

let mean (X:Matrix<_>) = 
    X.EnumerateColumns()
    |> Seq.map Statistics.Mean
    |> vector

let std (X:Matrix<_>) = 
    X.EnumerateColumns()
    |> Seq.map Statistics.StandardDeviation
    |> vector

let (./) (u:Vector<'a>) (v:Vector<'a>) = u.PointwiseDivide(v)

// 4 Feature normalisation
let featureNormalise (X:Matrix<_>) = 
    let mu = mean X
    let sigma = std X

    X.EnumerateRows()
    |> Seq.map (fun x -> (x - mu) ./ sigma)
    |> DenseMatrix.OfRowVectors

let addIntercept (X:Matrix<_>) = X.InsertColumn (0, DenseVector.Create(X.RowCount, 1.))

let Trans (x: Matrix<_>) = x.Transpose()
let Dim (v: Vector<_>) = float v.Count

// 2 & 5 Compute cost
let computeCost X y theta =
    let h = X * theta
    let m = Dim y
    let total = 
        Seq.zip h y
        |> Seq.map (fun (hi,yi) -> (hi - yi)**2.)
        |> Seq.sum
    total / (2. * float m)


// 3 & 6 Gradient descent
let gradientDescent (X:Matrix<_>) y (theta:Vector<_>) alpha numIters =
    let m = Dim y
    let rec doIteration X y theta costHistory alpha iters =
        if iters > numIters then
            (theta, costHistory)
        else
            let theta' = theta - (alpha/m) * Trans(X) * (X*theta - y)
            let J = computeCost X y theta'
            doIteration X y theta' ((J, theta')::costHistory) alpha (iters+1)
    doIteration X y theta [] alpha 1