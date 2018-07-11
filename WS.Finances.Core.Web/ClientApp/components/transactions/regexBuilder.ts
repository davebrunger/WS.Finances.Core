export class RegexBuilder {
    public static toPattern(source: string[]) {
        if (!source) {
            return null;
        }
        switch (source.length) {
        case 0:
            return "";
        case 1:
            return RegexBuilder.escapeRegexCharacters(source[0]);
        default:
            return `(${source.map(s => this.escapeRegexCharacters(s)).join("|")})`;
        }
    }

    private static escapeRegexCharacters(source: string) {
        const regexMetaCharacters = ["\\", "^", "[", ".", "$", "{", "*", "(", "+", ")", "|", "?", "<", ">"];
        for (let i = 0; i < regexMetaCharacters.length; i++) {
            const regexMetaCharacter = regexMetaCharacters[i];
            const regex = new RegExp(`\\${regexMetaCharacter}`, "g");
            source = source.replace(regex, `\\${regexMetaCharacter}`);
        }
        return source;
    }
}