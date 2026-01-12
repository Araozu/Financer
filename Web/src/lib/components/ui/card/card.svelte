<script lang="ts">
	import type { HTMLAttributes } from "svelte/elements";
	import { cn, type WithElementRef } from "$lib/utils.js";

	let {
		ref = $bindable(null),
		class: className,
		children,
		...restProps
	}: WithElementRef<HTMLAttributes<HTMLDivElement>> = $props();
</script>

<div
	bind:this={ref}
	data-slot="card"
	class={cn(
		"bg-glass-bg backdrop-blur-xl text-card-foreground flex flex-col gap-6 rounded-2xl border border-glass-border py-6 shadow-[0_4px_24px_-4px_var(--glass-shadow),inset_0_1px_1px_var(--glass-highlight)] transition-all duration-300",
		className
	)}
	{...restProps}
>
	{@render children?.()}
</div>
