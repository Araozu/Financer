{
  description = "Development environment with frontend, backend, and DevOps tools";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-25.05";
  };

  outputs = { self, nixpkgs }:
    let
      # Systems to support
      supportedSystems = [ "x86_64-linux" ];
      forAllSystems = nixpkgs.lib.genAttrs supportedSystems;
      pkgsFor = forAllSystems (system: import nixpkgs {
        inherit system;
        config = {};
        overlays = [];
      });
    in
    {
      devShells = forAllSystems (system:
        let
          pkgs = pkgsFor.${system};
        in
        {
          default = pkgs.mkShellNoCC {
            packages = with pkgs; [
              # Editors & tools
              neovim
              tmux

              # Frontend development
              nodejs_24
              pnpm

              # Backend development
              dotnetCorePackages.sdk_10_0-bin

              # DevOps & Infrastructure
              ansible
            ];
          };
        });
    };
}
