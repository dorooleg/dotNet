// 1. Посчитать числа Фибоначчи (за линейное время)
let fib n = 
    let rec fib' (n, a, b) =
        match n with
        | _ when n = 0I -> a
        | _ -> fib' ((n - 1I), b, (a + b))
    fib' (n, 0I, 1I)

// 2. Реализовать функцию обращения списка (за линейное время)
let rev l =
    let rec rev' l acc =
        match l with
        | [] -> acc
        | [x] -> x::acc
        | h::t -> rev' t (h::acc)
    rev' l []

// 3. Написать mergesort: функцию, которая принимает список и возвращает отсортированный список
let rec merge = function
    | ([], ys) -> ys
    | (xs, []) -> xs
    | (x::xs, y::ys) -> if x < y then x :: merge (xs, y::ys)
                        else y :: merge (x::xs, ys)

let rec split = function
    | [] -> ([], [])
    | [a] -> ([a], [])
    | a::b::cs -> let (L, R) = split cs 
                  (a::L, b::R)

let rec mergesort = function
    | [] -> []
    | [a] -> [a]
    | l -> let (L, R) = split l
           merge (mergesort L, mergesort R)

// 4. Посчитать значение дерева разбора арифметического выражения, заданного через вложенные discriminated union-ы
type Expression =
    | Number of int
    | Add of Expression * Expression
    | Sub of Expression * Expression
    | Multiply of Expression * Expression
    | Div of Expression * Expression
    | Mod of Expression * Expression

let rec Evaluate expr =
    match expr with
    | Number n -> n
    | Add (x, y) -> Evaluate x + Evaluate y
    | Sub (x, y) -> Evaluate x - Evaluate y
    | Multiply (x, y) -> Evaluate x * Evaluate y
    | Div (x, y) -> Evaluate x / Evaluate y
    | Mod (x, y) -> Evaluate x % Evaluate y

// 5. Реализовать функцию, генерирующую бесконечную последовательность простых чисел
let rec isprime x =
    primes
    |> Seq.takeWhile (fun i -> i * i <= x)
    |> Seq.forall (fun i -> x % i <> 0I)

and primes = 
    seq {
        yield 2I
        yield! Seq.unfold (fun i -> Some (i, i + 2I)) 3I |> Seq.filter isprime
    }


// --- Tests ---

// 1. Fib Test
assert (fib 0I = 0I)
assert (fib 1I = 1I)
assert (fib 10I = 55I)
fib 100000I |> ignore

// 2. Reverse Test
assert (rev List.empty<int> = List.empty<int>)
assert (rev [1] = [1])
assert (rev [1..1000000] = List.rev [1..1000000])

// 3. Mergesort Test
assert (mergesort [] = [])
assert (mergesort [1] = [1])
assert (mergesort [1; 2] = [1; 2])
assert (mergesort [2; 1] = [1; 2])
assert (mergesort [0; 2; 1; 3; 2] = [0; 1; 2; 2; 3])

// 4. Evaluate Expression Test
let tree = Add(Number 3, Multiply(Number 2, Div (Number 6, Sub(Number 4, Mod(Number 1, Number 2)))))
assert (Evaluate tree = 7)

// 5. Primes Test
assert (Seq.toList (Seq.take 0 primes) = [])
assert (Seq.toList (Seq.take 1 primes) = [2I])
assert (Seq.toList (Seq.take 2 primes) = [2I; 3I])
assert (Seq.toList (Seq.take 3 primes) = [2I; 3I; 5I])