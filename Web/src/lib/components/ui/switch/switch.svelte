<script lang="ts">
	import { Switch as SwitchPrimitive } from "bits-ui";
	import { cn, type WithoutChildrenOrChild } from "$lib/utils.js";

	let {
		ref = $bindable(null),
		class: className,
		checked = $bindable(false),
		...restProps
	}: WithoutChildrenOrChild<SwitchPrimitive.RootProps> = $props();
</script>

<SwitchPrimitive.Root
	bind:ref
	bind:checked
	data-slot="switch"
	class={cn(
		"data-[state=checked]:bg-primary data-[state=unchecked]:bg-glass-bg data-[state=unchecked]:backdrop-blur-lg focus-visible:border-ring focus-visible:ring-ring/50 dark:data-[state=unchecked]:bg-glass-bg shadow-[0_2px_8px_-2px_var(--glass-shadow),inset_0_1px_1px_var(--glass-highlight)] peer inline-flex h-[1.15rem] w-8 shrink-0 items-center rounded-full border border-glass-border outline-none transition-all duration-300 focus-visible:ring-[3px] disabled:cursor-not-allowed disabled:opacity-50",
		className
	)}
	{...restProps}
>
	<SwitchPrimitive.Thumb
		data-slot="switch-thumb"
		class={cn(
			"bg-background dark:data-[state=unchecked]:bg-foreground dark:data-[state=checked]:bg-primary-foreground pointer-events-none block size-4 rounded-full ring-0 transition-transform data-[state=checked]:translate-x-[calc(100%-2px)] data-[state=unchecked]:translate-x-0"
		)}
	/>
</SwitchPrimitive.Root>
