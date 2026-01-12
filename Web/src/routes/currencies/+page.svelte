<script lang="ts">
	import { Button } from "$lib/components/ui/button";
	import * as Card from "$lib/components/ui/card";
	import * as Dialog from "$lib/components/ui/dialog";
	import * as Table from "$lib/components/ui/table";
	import { Input } from "$lib/components/ui/input";
	import { Label } from "$lib/components/ui/label";
	import { Skeleton } from "$lib/components/ui/skeleton";
	import PlusIcon from "@lucide/svelte/icons/plus";
	import PencilIcon from "@lucide/svelte/icons/pencil";
	import TrashIcon from "@lucide/svelte/icons/trash-2";
	import {
		createCurrenciesQuery,
		createCurrencyMutation,
		updateCurrencyMutation,
		deleteCurrencyMutation,
		type Currency,
		type CreateCurrencyRequest,
	} from "$lib/currencies";

	const currenciesQuery = createCurrenciesQuery();
	const createMutation = createCurrencyMutation();
	const editMutation = updateCurrencyMutation();
	const removeMutation = deleteCurrencyMutation();

	let createDialogOpen = $state(false);
	let editDialogOpen = $state(false);
	let deleteDialogOpen = $state(false);

	let formData: CreateCurrencyRequest = $state({ code: "", name: "", symbol: "" });
	let editingCurrency: Currency | null = $state(null);
	let deletingCurrency: Currency | null = $state(null);

	function openCreateDialog() {
		formData = { code: "", name: "", symbol: "" };
		createDialogOpen = true;
	}

	function openEditDialog(currency: Currency) {
		editingCurrency = currency;
		formData = { code: currency.code, name: currency.name, symbol: currency.symbol };
		editDialogOpen = true;
	}

	function openDeleteDialog(currency: Currency) {
		deletingCurrency = currency;
		deleteDialogOpen = true;
	}

	async function handleCreate() {
		await $createMutation.mutateAsync(formData);
		createDialogOpen = false;
	}

	async function handleEdit() {
		if (!editingCurrency) return;
		await $editMutation.mutateAsync({ id: editingCurrency.id, data: formData });
		editDialogOpen = false;
		editingCurrency = null;
	}

	async function handleDelete() {
		if (!deletingCurrency) return;
		await $removeMutation.mutateAsync(deletingCurrency.id);
		deleteDialogOpen = false;
		deletingCurrency = null;
	}
</script>

<div class="container mx-auto px-4 py-8">
	<Card.Root class="px-6">
		<Card.Header class="flex-row items-center justify-between">
			<div>
				<Card.Title class="text-2xl">Currencies</Card.Title>
				<Card.Description>Manage your currencies</Card.Description>
			</div>
			<Button onclick={openCreateDialog}>
				<PlusIcon class="size-4" />
				Add Currency
			</Button>
		</Card.Header>
		<Card.Content>
			{#if $currenciesQuery.isPending}
				<div class="space-y-3">
					<Skeleton class="h-12 w-full" />
					<Skeleton class="h-12 w-full" />
					<Skeleton class="h-12 w-full" />
				</div>
			{:else if $currenciesQuery.error}
				<div class="rounded-xl border border-destructive/30 bg-destructive/10 p-4 text-destructive">
					Error loading currencies: {$currenciesQuery.error.detail ?? "Unknown error"}
				</div>
			{:else if $currenciesQuery.data}
				{#if $currenciesQuery.data.length === 0}
					<div class="py-12 text-center text-muted-foreground">
						No currencies yet. Click "Add Currency" to create one.
					</div>
				{:else}
					<Table.Root>
						<Table.Header>
							<Table.Row>
								<Table.Head>Code</Table.Head>
								<Table.Head>Name</Table.Head>
								<Table.Head>Symbol</Table.Head>
								<Table.Head class="w-24 text-right">Actions</Table.Head>
							</Table.Row>
						</Table.Header>
						<Table.Body>
							{#each $currenciesQuery.data as currency (currency.id)}
								<Table.Row>
									<Table.Cell class="font-mono font-medium">{currency.code}</Table.Cell>
									<Table.Cell>{currency.name}</Table.Cell>
									<Table.Cell class="font-mono">{currency.symbol}</Table.Cell>
									<Table.Cell class="text-right">
										<div class="flex justify-end gap-2">
											<Button
												variant="ghost"
												size="icon-sm"
												onclick={() => openEditDialog(currency)}
											>
												<PencilIcon class="size-4" />
											</Button>
											<Button
												variant="ghost"
												size="icon-sm"
												onclick={() => openDeleteDialog(currency)}
											>
												<TrashIcon class="size-4" />
											</Button>
										</div>
									</Table.Cell>
								</Table.Row>
							{/each}
						</Table.Body>
					</Table.Root>
				{/if}
			{/if}
		</Card.Content>
	</Card.Root>
</div>

<!-- Create Dialog -->
<Dialog.Root bind:open={createDialogOpen}>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Create Currency</Dialog.Title>
			<Dialog.Description>Add a new currency to your system.</Dialog.Description>
		</Dialog.Header>
		<form onsubmit={(e) => { e.preventDefault(); handleCreate(); }} class="space-y-4">
			<div class="space-y-2">
				<Label for="create-code">Code</Label>
				<Input
					id="create-code"
					placeholder="USD"
					bind:value={formData.code}
					required
					maxlength={10}
				/>
			</div>
			<div class="space-y-2">
				<Label for="create-name">Name</Label>
				<Input
					id="create-name"
					placeholder="US Dollar"
					bind:value={formData.name}
					required
					maxlength={100}
				/>
			</div>
			<div class="space-y-2">
				<Label for="create-symbol">Symbol</Label>
				<Input
					id="create-symbol"
					placeholder="$"
					bind:value={formData.symbol}
					required
					maxlength={10}
				/>
			</div>
			<Dialog.Footer>
				<Button variant="outline" type="button" onclick={() => (createDialogOpen = false)}>
					Cancel
				</Button>
				<Button type="submit" disabled={$createMutation.isPending}>
					{$createMutation.isPending ? "Creating..." : "Create"}
				</Button>
			</Dialog.Footer>
		</form>
	</Dialog.Content>
</Dialog.Root>

<!-- Edit Dialog -->
<Dialog.Root bind:open={editDialogOpen}>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Edit Currency</Dialog.Title>
			<Dialog.Description>Update the currency details.</Dialog.Description>
		</Dialog.Header>
		<form onsubmit={(e) => { e.preventDefault(); handleEdit(); }} class="space-y-4">
			<div class="space-y-2">
				<Label for="edit-code">Code</Label>
				<Input
					id="edit-code"
					placeholder="USD"
					bind:value={formData.code}
					required
					maxlength={10}
				/>
			</div>
			<div class="space-y-2">
				<Label for="edit-name">Name</Label>
				<Input
					id="edit-name"
					placeholder="US Dollar"
					bind:value={formData.name}
					required
					maxlength={100}
				/>
			</div>
			<div class="space-y-2">
				<Label for="edit-symbol">Symbol</Label>
				<Input
					id="edit-symbol"
					placeholder="$"
					bind:value={formData.symbol}
					required
					maxlength={10}
				/>
			</div>
			<Dialog.Footer>
				<Button variant="outline" type="button" onclick={() => (editDialogOpen = false)}>
					Cancel
				</Button>
				<Button type="submit" disabled={$editMutation.isPending}>
					{$editMutation.isPending ? "Saving..." : "Save"}
				</Button>
			</Dialog.Footer>
		</form>
	</Dialog.Content>
</Dialog.Root>

<!-- Delete Confirmation Dialog -->
<Dialog.Root bind:open={deleteDialogOpen}>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Delete Currency</Dialog.Title>
			<Dialog.Description>
				Are you sure you want to delete <span class="font-semibold">{deletingCurrency?.name}</span> ({deletingCurrency?.code})?
				This action cannot be undone.
			</Dialog.Description>
		</Dialog.Header>
		<Dialog.Footer>
			<Button variant="outline" onclick={() => (deleteDialogOpen = false)}>Cancel</Button>
			<Button variant="destructive" onclick={handleDelete} disabled={$removeMutation.isPending}>
				{$removeMutation.isPending ? "Deleting..." : "Delete"}
			</Button>
		</Dialog.Footer>
	</Dialog.Content>
</Dialog.Root>
