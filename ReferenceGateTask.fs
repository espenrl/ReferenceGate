// https://natemcmaster.com/blog/2017/07/05/msbuild-task-in-nuget/
// http://blog.differentpla.net/blog/2013/02/01/msbuild-tasks-input-parameters-and-itemgroups

namespace ReferenceGate

open Microsoft.Build.Framework

type private Reference = { Name: string }

type ReferenceGateTask() =
    inherit Microsoft.Build.Utilities.Task()

    [<Required>] member val Type : string = "" with get, set
    [<Required>] member val ActualReferences   : ITaskItem[] = Array.empty<ITaskItem> with get, set
    [<Required>] member val ReferenceWhitelist : ITaskItem[] = Array.empty<ITaskItem> with get, set
    [<Required>] member val ReferenceBlacklist : ITaskItem[] = Array.empty<ITaskItem> with get, set

    override __.Execute() =
        
        let map (refs:ITaskItem[]) =
            refs
            |> Array.map (fun r -> { Name = r.ItemSpec })
            |> Set.ofArray

        let a = __.ActualReferences |> map
        let w = __.ReferenceWhitelist |> map
        let b = __.ReferenceBlacklist |> map

        match (w.Count, b.Count) with
        | (0, 0) ->
            true
        | (wlen, 0) when wlen > 0 ->
            let diff = w |> Set.difference a
            diff |> Set.iter (fun ref -> __.Log.LogError(sprintf "%s '%s' is not on whitelist." __.Type ref.Name))
            diff |> Set.isEmpty
        | (0, blen) when blen > 0 ->
            let diff = a |> Set.intersect b
            diff |> Set.iter (fun ref -> __.Log.LogError(sprintf "%s '%s' is blacklisted." __.Type ref.Name))
            diff |> Set.isEmpty
        | (_, _) ->
            __.Log.LogError("Can not whitelist and blacklist at the same time. Choose either method.")
            false
