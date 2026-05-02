## graphify

This project has a graphify knowledge graph at `graphify-out/graph.json`.
The graph is the **single source of truth** for architecture and codebase navigation.

## MANDATORY: Graph-First Protocol

**NEVER read raw source files to understand architecture.** Always follow this order:

1. **Query the MCP graph first** using `query_graph`, `get_node`, `get_neighbors`, `shortest_path`, or `god_nodes`
2. **Check `graphify-out/GRAPH_REPORT.md`** for a plain-language overview of communities and god nodes
3. **Only open a specific source file** when you need to read/edit its exact implementation — never to explore structure

## MCP Tools Available (prefer over grep/list_dir for architecture questions)

| Tool | Use When |
|------|----------|
| `query_graph "<question>"` | Understanding how a feature works end-to-end |
| `get_node "<name>"` | Getting full details about a class, interface, or file |
| `get_neighbors "<name>"` | Seeing what a node depends on or is depended by |
| `shortest_path "<A>" "<B>"` | Tracing how two components connect |
| `god_nodes` | Identifying the most central/important nodes |
| `graph_stats` | Quick overview of graph size and confidence |

## Rules

- Before any architecture or codebase question → run `query_graph` or `get_node` first
- Before exploring a module → check `god_nodes` and `graph_stats` for orientation
- Do NOT use `list_dir`, `grep_search`, or `view_file` to explore structure — use the graph
- DO use `view_file` only after the graph tells you which exact file and line to look at
- After modifying code in this session → run `graphify update p:\dotnet-template` to keep the graph current (AST-only, no API cost)
- The correct update command is: `graphify update <absolute-path>` (NOT `graphify .`)
