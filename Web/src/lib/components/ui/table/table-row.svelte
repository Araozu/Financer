<script lang="ts">
	import { cn, type WithElementRef } from "$lib/utils.js";
	import type { HTMLAttributes } from "svelte/elements";

	let {
		ref = $bindable(null),
		class: className,
		children,
		...restProps
	}: WithElementRef<HTMLAttributes<HTMLTableRowElement>> = $props();
</script>

<tr
	bind:this={ref}
	data-slot="table-row"
	class={cn(
		"hover:[&,&>svelte-css-wrapper]:[&>th,td]:bg-glass-bg-hover data-[state=selected]:bg-glass-bg-active border-b border-glass-border/50 transition-all duration-200",
		className
	)}
	{...restProps}
>
	{@render children?.()}
</tr>
