import { Mapper } from "../utils/helpers";

export class ValidationResult {
    constructor(result?: ValidationResult) {
        if (result) {
            Mapper.map(result, this);
        }
    }

    public succeeded = false;
    public errors: string[] = null;
}