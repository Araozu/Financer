<script lang="ts">
	import type { HTMLInputAttributes, HTMLInputTypeAttribute } from "svelte/elements";
	import { cn, type WithElementRef } from "$lib/utils.js";

	type InputType = Exclude<HTMLInputTypeAttribute, "file">;

	type Props = WithElementRef<
		Omit<HTMLInputAttributes, "type"> &
			({ type: "file"; files?: FileList } | { type?: InputType; files?: undefined })
	>;

	let {
		ref = $bindable(null),
		value = $bindable(),
		type,
		files = $bindable(),
		class: className,
		"data-slot": dataSlot = "input",
		...restProps
	}: Props = $props();
</script>

{#if type === "file"}
	<input
		bind:this={ref}
		data-slot={dataSlot}
		class={cn(
			"selection:bg-primary bg-glass-bg backdrop-blur-lg selection:text-primary-foreground border-glass-border ring-offset-background placeholder:text-muted-foreground shadow-[0_2px_12px_-2px_var(--glass-shadow),inset_0_1px_1px_var(--glass-highlight)] flex h-9 w-full min-w-0 rounded-xl border px-3 pt-1.5 text-sm font-medium outline-none transition-all duration-300 disabled:cursor-not-allowed disabled:opacity-50",
			"focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px] focus-visible:bg-glass-bg-hover",
			"aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive",
			className
		)}
		type="file"
		bind:files
		bind:value
		{...restProps}
	/>
{:else}
	<input
		bind:this={ref}
		data-slot={dataSlot}
		class={cn(
			"bg-glass-bg backdrop-blur-lg selection:bg-primary selection:text-primary-foreground border-glass-border ring-offset-background placeholder:text-muted-foreground shadow-[0_2px_12px_-2px_var(--glass-shadow),inset_0_1px_1px_var(--glass-highlight)] flex h-9 w-full min-w-0 rounded-xl border px-3 py-1 text-base outline-none transition-all duration-300 disabled:cursor-not-allowed disabled:opacity-50 md:text-sm",
			"focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px] focus-visible:bg-glass-bg-hover",
			"aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive",
			className
		)}
		{type}
		bind:value
		{...restProps}
	/>
{/if}
